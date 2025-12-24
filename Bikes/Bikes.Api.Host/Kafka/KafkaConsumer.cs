using Bikes.Application.Interfaces;
using Bikes.Contracts.Dto;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Bikes.Api.Host.Kafka;

/// <summary>
/// Background service for consuming Kafka messages and processing contract DTOs
/// </summary>
public class KafkaConsumer : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<KafkaConsumer> _logger;
    private readonly KafkaConsumerOptions _options;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes the Kafka consumer with configuration and dependencies
    /// </summary>
    public KafkaConsumer(
        IConsumer<Ignore, string> consumer,
        IServiceScopeFactory scopeFactory,
        IOptions<KafkaConsumerOptions> options,
        ILogger<KafkaConsumer> logger)
    {
        _consumer = consumer;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _options = options.Value;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    /// <summary>
    /// Main execution method that consumes and processes Kafka messages
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Waiting for application to fully start (20 seconds)...");
        await Task.Delay(20000, stoppingToken);

        _logger.LogInformation("Starting KafkaConsumer...");

        try
        {
            _consumer.Subscribe(_options.Topic);
            _logger.LogInformation("KafkaConsumer subscribed to topic: {Topic}", _options.Topic);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to subscribe to Kafka topic");
            return;
        }


        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(5));

                if (consumeResult == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(consumeResult.Message?.Value))
                {
                    _logger.LogWarning("Received empty message");
                    continue;
                }

                _logger.LogDebug("Processing message at offset {Offset}",
                    consumeResult.TopicPartitionOffset);

                var contractType = DetermineContractType(consumeResult.Message.Headers);
                await ProcessMessageAsync(consumeResult.Message.Value, contractType, stoppingToken);

                _consumer.Commit(consumeResult);
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "Kafka consumption error: {Error}. Waiting 10 seconds...", ex.Error.Reason);
                await Task.Delay(10000, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("KafkaConsumer is stopping...");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in KafkaConsumer. Waiting 15 seconds...");
                await Task.Delay(15000, stoppingToken);
            }
        }

        _consumer.Close();
        _logger.LogInformation("KafkaConsumer stopped");
    }

    /// <summary>
    /// Determines the contract type from Kafka message headers
    /// </summary>
    /// <param name="headers">Kafka message headers</param>
    /// <returns>Contract type name or null if not found</returns>
    private string? DetermineContractType(Headers headers)
    {
        if (headers == null) return null;

        var contractTypeHeader = headers.FirstOrDefault(h => h.Key == "contract-type");
        if (contractTypeHeader != null)
        {
            return System.Text.Encoding.UTF8.GetString(contractTypeHeader.GetValueBytes());
        }

        return null;
    }

    /// <summary>
    /// Processes a Kafka message based on its contract type
    /// </summary>
    /// <param name="messageJson">JSON message content</param>
    /// <param name="contractType">Type of contract to process</param>
    private async Task ProcessMessageAsync(string messageJson, string? contractType, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        try
        {
            switch (contractType)
            {
                case "BikeCreateUpdateDto":
                    var bikeDto = JsonSerializer.Deserialize<BikeCreateUpdateDto>(messageJson, _jsonOptions);
                    if (bikeDto != null)
                    {
                        var bikeService = scope.ServiceProvider.GetRequiredService<IBikeService>();
                        var result = bikeService.CreateBike(bikeDto);
                        _logger.LogInformation("Created bike with ID: {BikeId}", result?.Id);
                    }
                    break;

                case "BikeModelCreateUpdateDto":
                    var bikeModelDto = JsonSerializer.Deserialize<BikeModelCreateUpdateDto>(messageJson, _jsonOptions);
                    if (bikeModelDto != null)
                    {
                        var bikeModelService = scope.ServiceProvider.GetRequiredService<IBikeModelService>();
                        var result = bikeModelService.CreateBikeModel(bikeModelDto);
                        _logger.LogInformation("Created bike model with ID: {ModelId}", result?.Id);
                    }
                    break;

                case "RenterCreateUpdateDto":
                    var renterDto = JsonSerializer.Deserialize<RenterCreateUpdateDto>(messageJson, _jsonOptions);
                    if (renterDto != null)
                    {
                        var renterService = scope.ServiceProvider.GetRequiredService<IRenterService>();
                        var result = renterService.CreateRenter(renterDto);
                        _logger.LogInformation("Created renter with ID: {RenterId}", result?.Id);
                    }
                    break;

                case "RentCreateUpdateDto":
                    var rentDto = JsonSerializer.Deserialize<RentCreateUpdateDto>(messageJson, _jsonOptions);
                    if (rentDto != null)
                    {
                        var rentService = scope.ServiceProvider.GetRequiredService<IRentService>();
                        var result = rentService.CreateRent(rentDto);
                        _logger.LogInformation("Created rent with ID: {RentId}", result?.Id);
                    }
                    break;

                default:
                    // Попробуем определить тип по структуре JSON
                    await TryAutoDetectAndProcessAsync(messageJson, scope, cancellationToken);
                    break;
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize message: {Message}", messageJson);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Validation error: {ErrorMessage}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
        }
    }

    /// <summary>
    /// Attempts to auto-detect contract type from JSON structure and process accordingly
    /// </summary>
    private async Task TryAutoDetectAndProcessAsync(string messageJson, IServiceScope scope, CancellationToken cancellationToken)
    {
        try
        {
            if (messageJson.Contains("SerialNumber") && messageJson.Contains("Color"))
            {
                var dto = JsonSerializer.Deserialize<BikeCreateUpdateDto>(messageJson, _jsonOptions);
                if (dto != null)
                {
                    var service = scope.ServiceProvider.GetRequiredService<IBikeService>();
                    service.CreateBike(dto);
                }
            }
            else if (messageJson.Contains("Type") && messageJson.Contains("WheelSize"))
            {
                var dto = JsonSerializer.Deserialize<BikeModelCreateUpdateDto>(messageJson, _jsonOptions);
                if (dto != null)
                {
                    var service = scope.ServiceProvider.GetRequiredService<IBikeModelService>();
                    service.CreateBikeModel(dto);
                }
            }
            else if (messageJson.Contains("FullName") && messageJson.Contains("Number"))
            {
                var dto = JsonSerializer.Deserialize<RenterCreateUpdateDto>(messageJson, _jsonOptions);
                if (dto != null)
                {
                    var service = scope.ServiceProvider.GetRequiredService<IRenterService>();
                    service.CreateRenter(dto);
                }
            }
            else if (messageJson.Contains("RentalStartTime") && messageJson.Contains("RentalDuration"))
            {
                var dto = JsonSerializer.Deserialize<RentCreateUpdateDto>(messageJson, _jsonOptions);
                if (dto != null)
                {
                    var service = scope.ServiceProvider.GetRequiredService<IRentService>();
                    service.CreateRent(dto);
                }
            }
            else
            {
                _logger.LogWarning("Could not determine contract type for message: {Message}", messageJson);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to auto-detect and process message");
        }
    }

    /// <summary>
    /// Disposes the Kafka consumer instance
    /// </summary>
    public override void Dispose()
    {
        _consumer?.Dispose();
        base.Dispose();
    }
}
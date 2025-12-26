using Bikes.Application.Interfaces;
using Bikes.Contracts.Dto;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Bikes.Api.Host.Kafka;

/// <summary>
/// Background service for consuming Kafka messages and processing contract DTOs
/// </summary>
public class KafkaConsumer(
    IConsumer<Ignore, string> consumer,
    IServiceScopeFactory scopeFactory,
    IOptions<KafkaConsumerOptions> options,
    ILogger<KafkaConsumer> logger) : BackgroundService
{
    private readonly KafkaConsumerOptions _options = options.Value;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Main execution method that consumes and processes Kafka messages
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = Task.Run(async () =>
        {
            await Task.Delay(5000, stoppingToken);

            logger.LogInformation("Starting KafkaConsumer in background thread...");

            try
            {
                consumer.Subscribe(_options.Topic);
                logger.LogInformation("KafkaConsumer subscribed to topic: {Topic}", _options.Topic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to subscribe to Kafka topic");
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

                    if (consumeResult == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(consumeResult.Message?.Value))
                    {
                        logger.LogWarning("Received empty message");
                        continue;
                    }

                    logger.LogDebug("Processing message at offset {Offset}",
                        consumeResult.TopicPartitionOffset);

                    var contractType = DetermineContractType(consumeResult.Message.Headers);
                    ProcessMessage(consumeResult.Message.Value, contractType);

                    consumer.Commit(consumeResult);
                }
                catch (ConsumeException ex)
                {
                    logger.LogError(ex, "Kafka consumption error: {Error}. Waiting 10 seconds...", ex.Error.Reason);
                    await Task.Delay(10000, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation("KafkaConsumer is stopping...");
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unexpected error in KafkaConsumer. Waiting 15 seconds...");
                    await Task.Delay(15000, stoppingToken);
                }
            }

            consumer.Close();
            logger.LogInformation("KafkaConsumer stopped");
        }, stoppingToken);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Determines the contract type from Kafka message headers
    /// </summary>
    /// <param name="headers">Kafka message headers</param>
    /// <returns>Contract type name or null if not found</returns>
    private static string? DetermineContractType(Headers headers)
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
    private void ProcessMessage(string messageJson, string? contractType)
    {
        using var scope = scopeFactory.CreateScope();

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
                        logger.LogInformation("Created bike with ID: {BikeId}", result?.Id);
                    }
                    break;

                case "BikeModelCreateUpdateDto":
                    var bikeModelDto = JsonSerializer.Deserialize<BikeModelCreateUpdateDto>(messageJson, _jsonOptions);
                    if (bikeModelDto != null)
                    {
                        var bikeModelService = scope.ServiceProvider.GetRequiredService<IBikeModelService>();
                        var result = bikeModelService.CreateBikeModel(bikeModelDto);
                        logger.LogInformation("Created bike model with ID: {ModelId}", result?.Id);
                    }
                    break;

                case "RenterCreateUpdateDto":
                    var renterDto = JsonSerializer.Deserialize<RenterCreateUpdateDto>(messageJson, _jsonOptions);
                    if (renterDto != null)
                    {
                        var renterService = scope.ServiceProvider.GetRequiredService<IRenterService>();
                        var result = renterService.CreateRenter(renterDto);
                        logger.LogInformation("Created renter with ID: {RenterId}", result?.Id);
                    }
                    break;

                case "RentCreateUpdateDto":
                    var rentDto = JsonSerializer.Deserialize<RentCreateUpdateDto>(messageJson, _jsonOptions);
                    if (rentDto != null)
                    {
                        var rentService = scope.ServiceProvider.GetRequiredService<IRentService>();
                        var result = rentService.CreateRent(rentDto);
                        logger.LogInformation("Created rent with ID: {RentId}", result?.Id);
                    }
                    break;

                default:
                    TryAutoDetectAndProcess(messageJson, scope);
                    break;
            }
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to deserialize message: {Message}", messageJson);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning("Validation error: {ErrorMessage}", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing message");
        }
    }

    /// <summary>
    /// Attempts to auto-detect contract type from JSON structure and process accordingly
    /// </summary>
    private void TryAutoDetectAndProcess(string messageJson, IServiceScope scope)
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
                logger.LogWarning("Could not determine contract type for message: {Message}", messageJson);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to auto-detect and process message");
        }
    }

    /// <summary>
    /// Disposes the Kafka consumer instance
    /// </summary>
    public override void Dispose()
    {
        try
        {
            if (consumer != null)
            {
                consumer.Close();
                consumer.Dispose();
            }
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Error while disposing Kafka consumer");
        }
        finally
        {
            base.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
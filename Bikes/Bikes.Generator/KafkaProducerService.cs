using Bikes.Generator.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Bikes.Generator;

public class KafkaProducerService : BackgroundService
{
    private readonly GeneratorOptions _generatorOptions;
    private readonly ContractGenerator _contractGenerator;
    private readonly IKafkaProducerFactory _producerFactory;
    private readonly ILogger<KafkaProducerService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public KafkaProducerService(
        IOptions<GeneratorOptions> generatorOptions,
        ContractGenerator contractGenerator,
        IKafkaProducerFactory producerFactory,
        ILogger<KafkaProducerService> logger)
    {
        _generatorOptions = generatorOptions.Value;
        _contractGenerator = contractGenerator;
        _producerFactory = producerFactory;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Kafka Producer Service");
        _logger.LogInformation("Configuration: Interval={IntervalMs}ms, BatchSize={BatchSize}, Topic={Topic}",
            _generatorOptions.IntervalMs, _generatorOptions.BatchSize, _generatorOptions.Topic);

        if (!_generatorOptions.Enabled)
        {
            _logger.LogInformation("Generator is disabled. Service will not produce messages.");
            return;
        }

        var producer = _producerFactory.CreateProducer();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var batch = _contractGenerator.GenerateBatch(_generatorOptions.BatchSize);

                foreach (var contract in batch)
                {
                    var message = CreateKafkaMessage(contract);
                    if (message != null)
                    {
                        await ProduceMessageAsync(producer, message, stoppingToken);
                    }
                }

                _logger.LogDebug("Generated and sent batch of {Count} messages", batch.Count);

                await Task.Delay(_generatorOptions.IntervalMs, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Producer service is stopping...");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in producer service");
                await Task.Delay(5000, stoppingToken);
            }
        }

        producer.Flush(stoppingToken);
        _logger.LogInformation("Kafka Producer Service stopped");
    }

    private Message<Null, string>? CreateKafkaMessage(object contract)
    {
        try
        {
            var json = JsonSerializer.Serialize(contract, contract.GetType(), _jsonOptions);
            var message = new Message<Null, string> { Value = json };

            // Добавляем метаданные в headers для определения типа контракта
            message.Headers = new Headers
            {
                new Header("contract-type", System.Text.Encoding.UTF8.GetBytes(contract.GetType().Name))
            };

            return message;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to serialize contract: {ContractType}", contract.GetType().Name);
            return null;
        }
    }

    private async Task ProduceMessageAsync(
        IProducer<Null, string> producer,
        Message<Null, string> message,
        CancellationToken cancellationToken)
    {
        try
        {
            var deliveryResult = await producer.ProduceAsync(
                _generatorOptions.Topic,
                message,
                cancellationToken);

            _logger.LogDebug("Message delivered to {Topic} [{Partition}] @ {Offset}",
                deliveryResult.Topic,
                deliveryResult.Partition,
                deliveryResult.Offset);
        }
        catch (ProduceException<Null, string> ex)
        {
            _logger.LogError(ex, "Failed to deliver message: {Error}", ex.Error.Reason);
        }
    }
}
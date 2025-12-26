using Bikes.Generator.Options;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Bikes.Generator;

/// <summary>
/// Background service for generating and publishing fake data to Kafka
/// </summary>
public class KafkaProducerService(
    IOptions<GeneratorOptions> generatorOptions,
    IOptions<KafkaOptions> kafkaOptions,
    ContractGenerator contractGenerator,
    IKafkaProducerFactory producerFactory,
    ILogger<KafkaProducerService> logger) : BackgroundService
{
    private readonly GeneratorOptions _generatorOptions = generatorOptions.Value;
    private readonly string _bootstrapServers = kafkaOptions.Value.BootstrapServers;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    /// <summary>
    /// Gets the Kafka bootstrap servers from environment variable or configuration
    /// </summary>
    /// <returns>Bootstrap servers connection string</returns>
    private string GetBootstrapServers()
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__kafka");
        if (!string.IsNullOrEmpty(connectionString))
            return connectionString;

        return _bootstrapServers;
    }

    /// <summary>
    /// Main execution method that generates and publishes data to Kafka
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting Kafka Producer Service");
        logger.LogInformation("Configuration: Interval={IntervalMs}ms, BatchSize={BatchSize}, Topic={Topic}",
            _generatorOptions.IntervalMs, _generatorOptions.BatchSize, _generatorOptions.Topic);

        if (!_generatorOptions.Enabled)
        {
            logger.LogInformation("Generator is disabled. Service will not produce messages.");
            return;
        }

        await CreateTopicIfNotExistsAsync(stoppingToken);

        var producer = producerFactory.CreateProducer();

        await Task.Delay(2000, stoppingToken);

        logger.LogInformation("Starting message generation...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var batch = contractGenerator.GenerateBatch(_generatorOptions.BatchSize);

                foreach (var contract in batch)
                {
                    var message = CreateKafkaMessage(contract);
                    if (message != null)
                    {
                        await ProduceMessageAsync(producer, message, stoppingToken);
                    }
                }

                logger.LogDebug("Generated and sent batch of {Count} messages", batch.Count);

                await Task.Delay(_generatorOptions.IntervalMs, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Producer service is stopping...");
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in producer service");
                await Task.Delay(5000, stoppingToken);
            }
        }

        producer.Flush(stoppingToken);
        logger.LogInformation("Kafka Producer Service stopped");
    }

    /// <summary>
    /// Creates the Kafka topic if it doesn't already exist
    /// </summary>
    private async Task CreateTopicIfNotExistsAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking if topic '{Topic}' exists...", _generatorOptions.Topic);

            var bootstrapServers = GetBootstrapServers();

            logger.LogInformation("Using Kafka at: {BootstrapServers}", bootstrapServers);

            using var adminClient = new AdminClientBuilder(new AdminClientConfig
            {
                BootstrapServers = bootstrapServers,
                ApiVersionRequest = false,
                BrokerVersionFallback = "0.10.0.0",
                SecurityProtocol = SecurityProtocol.Plaintext
            }).Build();

            try
            {
                var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
                var topicExists = metadata.Topics.Any(t => t.Topic == _generatorOptions.Topic && !t.Error.IsError);

                if (topicExists)
                {
                    logger.LogInformation("Topic '{Topic}' already exists", _generatorOptions.Topic);
                    return;
                }
            }
            catch (KafkaException ex)
            {
                logger.LogWarning(ex, "Failed to get Kafka metadata");
            }

            logger.LogInformation("Creating topic '{Topic}'...", _generatorOptions.Topic);

            try
            {
                await adminClient.CreateTopicsAsync(
                [
                    new()
                    {
                        Name = _generatorOptions.Topic,
                        NumPartitions = 1,
                        ReplicationFactor = 1
                    }
                ]);

                logger.LogInformation("Topic '{Topic}' created successfully", _generatorOptions.Topic);

                await Task.Delay(3000, cancellationToken);
            }
            catch (CreateTopicsException ex) when (ex.Results[0].Error.Code == ErrorCode.TopicAlreadyExists)
            {
                logger.LogInformation("Topic '{Topic}' already exists", _generatorOptions.Topic);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error checking/creating topic. Will try to continue...");
        }
    }

    /// <summary>
    /// Creates a Kafka message from a contract object
    /// </summary>
    /// <param name="contract">The contract object to serialize</param>
    /// <returns>Kafka message or null if serialization fails</returns>
    private Message<Null, string>? CreateKafkaMessage(object contract)
    {
        try
        {
            var json = JsonSerializer.Serialize(contract, contract.GetType(), _jsonOptions);
            return new Message<Null, string>
            {
                Value = json,
                Headers =
                 [
                     new Header("contract-type", System.Text.Encoding.UTF8.GetBytes(contract.GetType().Name))
                 ]
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to serialize contract: {ContractType}", contract.GetType().Name);
            return null;
        }
    }

    /// <summary>
    /// Publishes a message to Kafka topic
    /// </summary>
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

            logger.LogDebug("Message delivered to {Topic} [{Partition}] @ {Offset}",
                deliveryResult.Topic,
                deliveryResult.Partition,
                deliveryResult.Offset);
        }
        catch (ProduceException<Null, string> ex)
        {
            logger.LogError(ex, "Failed to deliver message: {Error}", ex.Error.Reason);
        }
    }
}
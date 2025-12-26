using Bikes.Generator.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bikes.Generator;

/// <summary>
/// Factory implementation for creating and managing Kafka producer instances
/// </summary>
public class KafkaProducerFactory(
    IOptions<KafkaOptions> options,
    ILogger<KafkaProducerFactory> logger) : IKafkaProducerFactory, IDisposable
{
    private readonly KafkaOptions _options = options.Value;
    private IProducer<Null, string>? _producer;

    /// <summary>
    /// Gets the Kafka bootstrap servers from environment variable or configuration
    /// </summary>
    /// <returns>Bootstrap servers connection string</returns>
    private string GetBootstrapServers()
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__kafka");
        if (!string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }

        return _options.BootstrapServers;
    }

    /// <summary>
    /// Creates a Kafka producer with retry logic and connection validation
    /// </summary>
    /// <returns>Configured Kafka producer instance</returns>
    public IProducer<Null, string> CreateProducer()
    {
        if (_producer != null)
            return _producer;

        var bootstrapServers = GetBootstrapServers();

        Console.WriteLine($"Kafka Producer connecting to: {bootstrapServers}");

        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            EnableDeliveryReports = true,

            ApiVersionRequest = false,
            ApiVersionFallbackMs = 0,
            BrokerVersionFallback = "0.10.0.0",
            SecurityProtocol = SecurityProtocol.Plaintext,
            SslEndpointIdentificationAlgorithm = SslEndpointIdentificationAlgorithm.None,

            SocketTimeoutMs = 30000,
            MessageTimeoutMs = 30000,
            RequestTimeoutMs = 30000,

            EnableIdempotence = false,
            Acks = Acks.Leader
        };

        var retryCount = 0;
        while (retryCount < _options.MaxRetryAttempts)
        {
            try
            {
                _producer = new ProducerBuilder<Null, string>(config)
                    .SetLogHandler((_, message) =>
                        logger.LogInformation("Kafka: {Facility} - {Message}", message.Facility, message.Message))
                    .SetErrorHandler((_, error) =>
                        logger.LogError("Kafka Error: {Reason} (Code: {Code})", error.Reason, error.Code))
                    .Build();

                logger.LogInformation("Kafka producer connected successfully to {BootstrapServers}",
                    bootstrapServers);

                return _producer;
            }
            catch (Exception ex)
            {
                retryCount++;
                logger.LogWarning(ex,
                    "Failed to connect to Kafka (attempt {RetryCount}/{MaxRetries}). Retrying in {DelayMs}ms...",
                    retryCount, _options.MaxRetryAttempts, _options.RetryDelayMs);

                if (retryCount >= _options.MaxRetryAttempts)
                {
                    logger.LogError(ex, "Max retry attempts reached. Failed to connect to Kafka.");
                    throw;
                }

                Thread.Sleep(_options.RetryDelayMs);
            }
        }

        throw new InvalidOperationException("Failed to create Kafka producer");
    }

    /// <summary>
    /// Disposes the Kafka producer instance
    /// </summary>
    public void Dispose()
    {
        try
        {
            if (_producer != null)
            {
                _producer.Flush(TimeSpan.FromSeconds(5));
                _producer.Dispose();
                _producer = null;
            }
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Error while disposing Kafka producer");
        }
        finally
        {
            GC.SuppressFinalize(this);
        }
    }
}
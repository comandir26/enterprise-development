using Bikes.Generator.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bikes.Generator;

public class KafkaProducerFactory : IKafkaProducerFactory, IDisposable
{
    private readonly KafkaOptions _options;
    private readonly ILogger<KafkaProducerFactory> _logger;
    private IProducer<Null, string>? _producer;

    public KafkaProducerFactory(
        IOptions<KafkaOptions> options,
        ILogger<KafkaProducerFactory> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public IProducer<Null, string> CreateProducer()
    {
        if (_producer != null)
            return _producer;

        var config = new ProducerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            MessageTimeoutMs = _options.MessageTimeoutMs,
            EnableDeliveryReports = true
        };

        var retryCount = 0;
        while (retryCount < _options.MaxRetryAttempts)
        {
            try
            {
                _producer = new ProducerBuilder<Null, string>(config)
                    .SetLogHandler((_, message) =>
                        _logger.LogInformation("Kafka: {Facility} - {Message}", message.Facility, message.Message))
                    .SetErrorHandler((_, error) =>
                        _logger.LogError("Kafka Error: {Reason} (Code: {Code})", error.Reason, error.Code))
                    .Build();

                _logger.LogInformation("Kafka producer connected successfully to {BootstrapServers}",
                    _options.BootstrapServers);

                return _producer;
            }
            catch (Exception ex)
            {
                retryCount++;
                _logger.LogWarning(ex,
                    "Failed to connect to Kafka (attempt {RetryCount}/{MaxRetries}). Retrying in {DelayMs}ms...",
                    retryCount, _options.MaxRetryAttempts, _options.RetryDelayMs);

                if (retryCount >= _options.MaxRetryAttempts)
                {
                    _logger.LogError(ex, "Max retry attempts reached. Failed to connect to Kafka.");
                    throw;
                }

                Thread.Sleep(_options.RetryDelayMs);
            }
        }

        throw new InvalidOperationException("Failed to create Kafka producer");
    }

    public void Dispose()
    {
        _producer?.Dispose();
        _producer = null;
    }
}
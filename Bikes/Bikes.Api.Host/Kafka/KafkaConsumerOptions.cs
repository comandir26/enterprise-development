namespace Bikes.Api.Host.Kafka;

/// <summary>
/// Configuration options for Kafka consumer
/// </summary>
public class KafkaConsumerOptions
{
    /// <summary>
    /// Gets or sets the Kafka bootstrap servers address
    /// </summary>
    public string BootstrapServers { get; set; } = "localhost:9092";

    /// <summary>
    /// Gets or sets the topic name to consume from
    /// </summary>
    public string Topic { get; set; } = "bikes-contracts";

    /// <summary>
    /// Gets or sets the consumer group identifier
    /// </summary>
    public string GroupId { get; set; } = "bikes-api-consumer-group";

    /// <summary>
    /// Gets or sets a value indicating whether to enable automatic offset committing
    /// </summary>
    public bool EnableAutoCommit { get; set; } = false;

    /// <summary>
    /// Gets or sets the auto offset reset behavior (0: earliest, 1: latest, 2: error)
    /// </summary>
    public int AutoOffsetReset { get; set; } = 0;

    /// <summary>
    /// Gets or sets the maximum number of retry attempts for connection
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Gets or sets the delay between retry attempts in milliseconds
    /// </summary>
    public int RetryDelayMs { get; set; } = 1000;

    /// <summary>
    /// Gets or sets a value indicating whether to request API version from broker
    /// </summary>
    public bool ApiVersionRequest { get; set; } = false;

    /// <summary>
    /// Gets or sets the API version fallback timeout in milliseconds
    /// </summary>
    public int ApiVersionFallbackMs { get; set; } = 0;

    /// <summary>
    /// Gets or sets the broker version fallback string
    /// </summary>
    public string BrokerVersionFallback { get; set; } = "0.10.0.0";

    /// <summary>
    /// Gets or sets the security protocol for Kafka connection
    /// </summary>
    public string SecurityProtocol { get; set; } = "Plaintext";

    /// <summary>
    /// Gets or sets a value indicating whether to allow automatic topic creation
    /// </summary>
    public bool AllowAutoCreateTopics { get; set; } = false;
}
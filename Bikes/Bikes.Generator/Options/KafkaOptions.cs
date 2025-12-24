namespace Bikes.Generator.Options;

/// <summary>
/// Configuration options for Kafka producer
/// </summary>
public class KafkaOptions
{
    /// <summary>
    /// Gets or sets the Kafka bootstrap servers address
    /// </summary>
    public string BootstrapServers { get; set; } = "localhost:9092";

    /// <summary>
    /// Gets or sets the maximum number of retry attempts for failed operations
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Gets or sets the delay between retry attempts in milliseconds
    /// </summary>
    public int RetryDelayMs { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the message timeout in milliseconds
    /// </summary
    public int MessageTimeoutMs { get; set; } = 5000;
}
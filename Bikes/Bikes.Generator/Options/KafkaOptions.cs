namespace Bikes.Generator.Options;

public class KafkaOptions
{
    public string BootstrapServers { get; set; } = "localhost:9092";
    public int MaxRetryAttempts { get; set; } = 3;
    public int RetryDelayMs { get; set; } = 1000;
    public int MessageTimeoutMs { get; set; } = 5000;
}
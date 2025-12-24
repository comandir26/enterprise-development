namespace Bikes.Generator.Options;

/// <summary>
/// Configuration options for the bikes data generator
/// </summary>
public class GeneratorOptions
{
    /// <summary>
    /// Gets or sets the generation interval in milliseconds
    /// </summary>
    public int IntervalMs { get; set; } = 5000;

    /// <summary>
    /// Gets or sets the number of bike records to generate per batch
    /// </summary>
    public int BatchSize { get; set; } = 1;

    /// <summary>
    /// Gets or sets the Kafka topic name for publishing generated data
    /// </summary>
    public string Topic { get; set; } = "bikes-contracts";

    /// <summary>
    /// Gets or sets a value indicating whether the generator is enabled
    /// </summary>
    public bool Enabled { get; set; } = true;
}
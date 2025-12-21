namespace Bikes.Generator.Options;

public class GeneratorOptions
{
    public int IntervalMs { get; set; } = 5000;
    public int BatchSize { get; set; } = 1;
    public string Topic { get; set; } = "bikes-contracts";
    public bool Enabled { get; set; } = true;
}
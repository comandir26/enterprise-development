namespace Bikes.Infrastructure.MongoDb.Configuration;

/// <summary>
/// MongoDB Connection Settings
/// </summary>
public class MongoDbSettings
{
    public const string SectionName = "MongoDb";

    /// <summary>
    /// Connection string
    /// </summary>
    public required string ConnectionString { get; set; }

    /// <summary>
    /// Database Name
    /// </summary>
    public required string DatabaseName { get; set; }
}
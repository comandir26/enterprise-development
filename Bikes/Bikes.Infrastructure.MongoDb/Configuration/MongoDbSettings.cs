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
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Database Name
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;
}
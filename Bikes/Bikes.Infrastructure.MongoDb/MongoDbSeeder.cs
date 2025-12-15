using MongoDB.Driver;
using Bikes.Infrastructure.InMemory.Seeders;
using Microsoft.Extensions.Logging;

namespace Bikes.Infrastructure.MongoDb;

/// <summary>
/// A class for initializing initial data in MongoDB
/// </summary>
public class MongoDbSeeder
{

    private readonly MongoDbContext _context;

    private readonly ILogger<MongoDbSeeder> _logger;

    /// <summary>
    /// Constructor of the MongoDbSeeder class
    /// </summary>
    /// <param name="context">MongoDB context for working with the database</param>
    /// <param name="logger">Logger for recording diagnostic information</param>
    public MongoDbSeeder(MongoDbContext context, ILogger<MongoDbSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// The main method for filling the database with initial data
    /// </summary>
    /// <returns>Asynchronous task</returns>
    public async Task SeedAsync()
    {
        var hasModels = await _context.BikeModels.Find(_ => true).AnyAsync();

        if (hasModels)
        {
            _logger.LogInformation("Database already contains data. Skipping seeding.");
            return;
        }

        _logger.LogInformation("Starting MongoDB database seeding...");

        var models = InMemorySeeder.GetBikeModels();
        var bikes = InMemorySeeder.GetBikes();
        var renters = InMemorySeeder.GetRenters();
        var rents = InMemorySeeder.GetRents();

        if (models.Any())
        {
            await _context.BikeModels.InsertManyAsync(models);
            _logger.LogInformation("Inserted {Count} bike models into database", models.Count);
        }

        if (bikes.Any())
        {
            await _context.Bikes.InsertManyAsync(bikes);
            _logger.LogInformation("Inserted {Count} bikes into database", bikes.Count);
        }

        if (renters.Any())
        {
            await _context.Renters.InsertManyAsync(renters);
            _logger.LogInformation("Inserted {Count} renters into database", renters.Count);
        }

        if (rents.Any())
        {
            await _context.Rents.InsertManyAsync(rents);
            _logger.LogInformation("Inserted {Count} rents into database", rents.Count);
        }

        _logger.LogInformation("MongoDB database seeding completed successfully");
    }
}
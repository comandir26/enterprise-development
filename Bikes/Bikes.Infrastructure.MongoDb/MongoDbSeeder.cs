using Microsoft.EntityFrameworkCore;
using Bikes.Infrastructure.InMemory.Seeders;
using Microsoft.Extensions.Logging;

namespace Bikes.Infrastructure.MongoDb;

/// <summary>
/// A class for initializing initial data in MongoDB
/// </summary>
public class MongoDbSeeder
{
    private readonly BikesDbContext _context;
    private readonly ILogger<MongoDbSeeder> _logger;

    public MongoDbSeeder(BikesDbContext context, ILogger<MongoDbSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Bikes.AnyAsync())
        {
            _logger.LogInformation("Database already contains data. Skipping seeding.");
            return;
        }

        _logger.LogInformation("Starting MongoDB database seeding...");

        var models = InMemorySeeder.GetBikeModels();
        var bikes = InMemorySeeder.GetBikes();
        var renters = InMemorySeeder.GetRenters();
        var rents = InMemorySeeder.GetRents();

        await _context.BikeModels.AddRangeAsync(models);
        await _context.Bikes.AddRangeAsync(bikes);
        await _context.Renters.AddRangeAsync(renters);
        await _context.Rents.AddRangeAsync(rents);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeding completed.");
    }
}
using Microsoft.EntityFrameworkCore;
using Bikes.Infrastructure.InMemory.Seeders;
using Microsoft.Extensions.Logging;

namespace Bikes.Infrastructure.MongoDb;

/// <summary>
/// A class for initializing initial data in MongoDB
/// </summary>
public class MongoDbSeeder(
    BikesDbContext context,
    ILogger<MongoDbSeeder> logger)
{
    public async Task SeedAsync()
    {
        if (await context.Bikes.AnyAsync())
        {
            logger.LogInformation("Database already contains data. Skipping seeding.");
            return;
        }

        logger.LogInformation("Starting MongoDB database seeding...");

        var models = InMemorySeeder.GetBikeModels();
        var bikes = InMemorySeeder.GetBikes();
        var renters = InMemorySeeder.GetRenters();
        var rents = InMemorySeeder.GetRents();

        await context.BikeModels.AddRangeAsync(models);
        await context.Bikes.AddRangeAsync(bikes);
        await context.Renters.AddRangeAsync(renters);
        await context.Rents.AddRangeAsync(rents);

        await context.SaveChangesAsync();

        logger.LogInformation("Seeding completed.");
    }
}
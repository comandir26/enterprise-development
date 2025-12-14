using MongoDB.Driver;
using Bikes.Domain.Models;
using Bikes.Infrastructure.InMemory.Seeders;

namespace Bikes.Infrastructure.MongoDb;

/// <summary>
/// MongoDb seeder
/// </summary>
public class MongoDbSeeder
{
    private readonly MongoDbContext _context;

    public MongoDbSeeder(MongoDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var hasModels = await _context.BikeModels.Find(_ => true).AnyAsync();
        if (hasModels) return;

        Console.WriteLine("Seeding MongoDB with initial data...");

        var models = InMemorySeeder.GetBikeModels();
        var bikes = InMemorySeeder.GetBikes();
        var renters = InMemorySeeder.GetRenters();
        var rents = InMemorySeeder.GetRents();

        if (models.Any())
        {
            await _context.BikeModels.InsertManyAsync(models);
            Console.WriteLine($"Inserted {models.Count} bike models");
        }

        if (bikes.Any())
        {
            await _context.Bikes.InsertManyAsync(bikes);
            Console.WriteLine($"Inserted {bikes.Count} bikes");
        }

        if (renters.Any())
        {
            await _context.Renters.InsertManyAsync(renters);
            Console.WriteLine($"Inserted {renters.Count} renters");
        }

        if (rents.Any())
        {
            await _context.Rents.InsertManyAsync(rents);
            Console.WriteLine($"Inserted {rents.Count} rents");
        }

        Console.WriteLine("Seeding completed!");
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Bikes.Domain.Repositories;
using Bikes.Infrastructure.MongoDb.Configuration;
using Bikes.Infrastructure.MongoDb.Repositories;

namespace Bikes.Infrastructure.MongoDb.Extensions;

/// <summary>
/// A class for hidden registration of services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// A method for registering MongoDB services through the Entity Framework Core
    /// </summary>
    /// <param name="services">Collection of services</param>
    /// <param name="configuration">Application Configuration</param>
    /// <returns>Collection of services</returns>
    public static IServiceCollection AddMongoDbInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB");
        var databaseName = configuration["MongoDb:DatabaseName"]!;

        services.Configure<MongoDbSettings>(options =>
        {
            options.ConnectionString = connectionString
                ?? "mongodb://localhost:27017";
            options.DatabaseName = databaseName;
        });

        services.AddDbContext<BikesDbContext>(options =>
        {
            options.UseMongoDB(connectionString ?? "mongodb://localhost:27017", databaseName);
        });

        services.AddScoped<MongoDbSeeder>();

        services.AddScoped<IRepository<Domain.Models.Bike, int>, MongoBikeRepository>();
        services.AddScoped<IRepository<Domain.Models.BikeModel, int>, MongoBikeModelRepository>();
        services.AddScoped<IRepository<Domain.Models.Renter, int>, MongoRenterRepository>();
        services.AddScoped<IRepository<Domain.Models.Rent, int>, MongoRentRepository>();

        return services;
    }
}
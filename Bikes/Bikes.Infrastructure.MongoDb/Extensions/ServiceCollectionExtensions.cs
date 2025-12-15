using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    /// The method that registers services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDbInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB");
        var databaseName = configuration["MongoDb:DatabaseName"] ?? "BikesDB";

        services.Configure<MongoDbSettings>(options =>
        {
            options.ConnectionString = connectionString
                ?? "mongodb://localhost:27017";
            options.DatabaseName = databaseName;
        });

        services.AddSingleton<MongoDbContext>(); 
        services.AddSingleton<MongoDbSeeder>();

        services.AddSingleton<IRepository<Domain.Models.Bike, int>, MongoBikeRepository>();
        services.AddSingleton<IRepository<Domain.Models.BikeModel, int>, MongoBikeModelRepository>();
        services.AddSingleton<IRepository<Domain.Models.Renter, int>, MongoRenterRepository>();
        services.AddSingleton<IRepository<Domain.Models.Rent, int>, MongoRentRepository>();

        return services;
    }
}
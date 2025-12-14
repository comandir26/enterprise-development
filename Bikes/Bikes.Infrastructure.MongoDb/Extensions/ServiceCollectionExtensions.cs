using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Bikes.Domain.Repositories;
using Bikes.Infrastructure.MongoDb.Configuration;
using Bikes.Infrastructure.MongoDb.Repositories;

namespace Bikes.Infrastructure.MongoDb.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDbInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var mongoDbSection = configuration.GetSection(MongoDbSettings.SectionName);
        var mongoDbSettings = new MongoDbSettings
        {
            ConnectionString = mongoDbSection["ConnectionString"]!,
            DatabaseName = mongoDbSection["DatabaseName"]!
        };

        services.AddSingleton(mongoDbSettings);
        services.AddSingleton<MongoDbContext>();
        services.AddSingleton<MongoDbSeeder>();

        services.AddSingleton<IRepository<Domain.Models.Bike, int>, MongoBikeRepository>();
        services.AddSingleton<IRepository<Domain.Models.BikeModel, int>, MongoBikeModelRepository>();
        services.AddSingleton<IRepository<Domain.Models.Renter, int>, MongoRenterRepository>();
        services.AddSingleton<IRepository<Domain.Models.Rent, int>, MongoRentRepository>();

        return services;
    }
}
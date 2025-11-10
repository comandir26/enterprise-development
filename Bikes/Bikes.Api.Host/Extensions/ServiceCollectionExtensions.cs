using Bikes.Application.Services;
using Bikes.Domain.Repositories;
using Bikes.Infrastructure.InMemory.Repositories;

namespace Bikes.Api.Host.Extensions;

/// <summary>
/// A class for hidden registration of services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// The method that registers services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBikeRentalServices(this IServiceCollection services)
    {
        services.AddSingleton<IRepository<Bikes.Domain.Models.Bike, int>, InMemoryBikeRepository>();
        services.AddSingleton<IRepository<Bikes.Domain.Models.BikeModel, int>, InMemoryBikeModelRepository>();
        services.AddSingleton<IRepository<Bikes.Domain.Models.Renter, int>, InMemoryRenterRepository>();
        services.AddSingleton<IRepository<Bikes.Domain.Models.Rent, int>, InMemoryRentRepository>();

        services.AddScoped<IBikeService, BikeService>();
        services.AddScoped<IBikeModelService, BikeModelService>();
        services.AddScoped<IRenterService, RenterService>();
        services.AddScoped<IRentService, RentService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();

        return services;
    }
}
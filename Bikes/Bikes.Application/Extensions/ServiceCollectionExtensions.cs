using Bikes.Application.Interfaces;
using Bikes.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bikes.Application.Extensions;

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
        services.AddAutoMapper(typeof(BikeService).Assembly);

        services.AddScoped<IBikeService, BikeService>();
        services.AddScoped<IBikeModelService, BikeModelService>();
        services.AddScoped<IRenterService, RenterService>();
        services.AddScoped<IRentService, RentService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();

        return services;
    }
}
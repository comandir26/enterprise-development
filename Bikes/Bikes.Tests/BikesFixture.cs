using Bikes.Application.Services;
using Bikes.Domain.Repositories;
using Bikes.Infrastructure.InMemory.Repositories;

namespace Bikes.Tests;

/// <summary>
/// A class for tests
/// </summary>
public class BikesFixture
{
    public readonly IAnalyticsService AnalyticsService;

    /// <summary>
    /// A constructor that creates repositories and service classes
    /// </summary>
    public BikesFixture()
    {
        IRepository<Bikes.Domain.Models.Bike, int> bikeRepo = new InMemoryBikeRepository();
        IRepository<Bikes.Domain.Models.BikeModel, int> modelRepo = new InMemoryBikeModelRepository();
        IRepository<Bikes.Domain.Models.Rent, int> rentRepo = new InMemoryRentRepository();
        IRepository<Bikes.Domain.Models.Renter, int> renterRepo = new InMemoryRenterRepository();

        AnalyticsService = new AnalyticsService(bikeRepo, modelRepo, rentRepo, renterRepo);
    }
}
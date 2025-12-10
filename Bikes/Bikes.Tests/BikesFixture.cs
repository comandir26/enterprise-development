using AutoMapper;
using Bikes.Application.Interfaces;
using Bikes.Application.Services;
using Bikes.Application.Mapping;
using Bikes.Domain.Repositories;
using Bikes.Infrastructure.InMemory.Repositories;

namespace Bikes.Tests;

/// <summary>
/// A class for tests
/// </summary>
public class BikesFixture
{
    public readonly IAnalyticsService AnalyticsService;
    private readonly IMapper _mapper;

    /// <summary>
    /// A constructor that creates repositories and service classes
    /// </summary>
    public BikesFixture()
    {
        IRepository<Domain.Models.Bike, int> bikeRepo = new InMemoryBikeRepository();
        IRepository<Domain.Models.BikeModel, int> modelRepo = new InMemoryBikeModelRepository();
        IRepository<Domain.Models.Rent, int> rentRepo = new InMemoryRentRepository();
        IRepository<Domain.Models.Renter, int> renterRepo = new InMemoryRenterRepository();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = configuration.CreateMapper();

        AnalyticsService = new AnalyticsService(bikeRepo, modelRepo, rentRepo, renterRepo, _mapper);
    }
}

using AutoMapper;
using Bikes.Application.Interfaces;
using Bikes.Contracts.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

/// <summary>
/// A class that implements the interface of the AnalyticsService class
/// </summary>
public class AnalyticsService(
    IRepository<Bike, int> bikeRepository,
    IRepository<BikeModel, int> bikeModelRepository,
    IRepository<Rent, int> rentRepository,
    IRepository<Renter, int> renterRepository,
    IMapper mapper) : IAnalyticsService
{
    /// <summary>
    /// A method that returns information about all sports bikes
    /// </summary>
    public List<BikeGetDto> GetSportBikes()
    {
        var sportBikes = bikeRepository.ReadAll()
            .Where(bike => bike.Model.Type == BikeType.Sport)
            .ToList();

        return mapper.Map<List<BikeGetDto>>(sportBikes);
    }

    /// <summary>
    /// A method that returns the top 5 bike models by rental duration
    /// </summary>
    public List<BikeModelGetDto> GetTopFiveModelsByRentDuration()
    {
        var topModels = rentRepository.ReadAll()
            .GroupBy(rent => rent.Bike.Model) 
            .Select(group => new
            {
                Model = group.Key,
                TotalDuration = group.Sum(rent => rent.RentalDuration)
            })
            .OrderByDescending(x => x.TotalDuration)
            .Take(5)
            .Select(x => x.Model)
            .ToList();

        return mapper.Map<List<BikeModelGetDto>>(topModels);
    }

    /// <summary>
    /// A method that returns the top 5 bike models in terms of rental income
    /// </summary>
    public List<BikeModelGetDto> GetTopFiveModelsByProfit()
    {
        var topModels = rentRepository.ReadAll()
            .GroupBy(rent => rent.Bike.Model)
            .Select(group => new
            {
                Model = group.Key,
                TotalProfit = group.Sum(rent => rent.RentalDuration * rent.Bike.Model.RentPrice)
            })
            .OrderByDescending(x => x.TotalProfit)
            .Take(5)
            .Select(x => x.Model)
            .ToList();

        return mapper.Map<List<BikeModelGetDto>>(topModels);
    }

    /// <summary>
    /// A method that returns information about the minimum, maximum, and average bike rental time.
    /// </summary>
    public RentalDurationStatsDto GetRentalDurationStats()
    {
        var durations = rentRepository.ReadAll()
            .Select(rent => rent.RentalDuration)
            .ToList();

        return new RentalDurationStatsDto
        {
            Min = durations.Min(),
            Max = durations.Max(),
            Average = durations.Average()
        };
    }

    /// <summary>
    /// A method that returns the total rental time of each type of bike
    /// </summary>
    public Dictionary<BikeType, int> GetTotalRentalTimeByType()
    {
        return rentRepository.ReadAll()
            .GroupBy(rent => rent.Bike.Model.Type)
            .ToDictionary(
                group => group.Key,
                group => group.Sum(rent => rent.RentalDuration)
            );
    }

    /// <summary>
    /// A method that returns information about the customers who have rented bicycles the most times.
    /// </summary>
    public List<RenterGetDto> GetTopThreeRenters()
    {
        var renters = renterRepository.ReadAll();

        var topRenters = rentRepository.ReadAll()
            .GroupBy(rent => rent.Renter.Id)
            .Select(group => new
            {
                RenterId = group.Key,
                TotalRentals = group.Count()
            })
            .OrderByDescending(r => r.TotalRentals)
            .Take(3)
            .Join(renters,
                  x => x.RenterId,
                  renter => renter.Id,
                  (x, renter) => renter)
            .ToList();

        return mapper.Map<List<RenterGetDto>>(topRenters);
    }
}
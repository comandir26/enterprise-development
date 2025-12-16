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
    IRepository<Rent, int> rentRepository,
    IRepository<Renter, int> renterRepository,
    IRepository<BikeModel, int> bikeModelRepository,
    IMapper mapper) : IAnalyticsService
{
    /// <summary>
    /// A method that returns information about all sports bikes
    /// </summary>
    public List<BikeGetDto> GetSportBikes()
    {
        var allBikes = bikeRepository.ReadAll();
        var allModels = bikeModelRepository.ReadAll();
        var modelDict = allModels.ToDictionary(m => m.Id);
        foreach (var bike in allBikes)
        {
            if (modelDict.TryGetValue(bike.ModelId, out var model))
                bike.Model = model;
        }

        var sportBikes = allBikes
            .Where(bike => bike.Model != null && bike.Model.Type == BikeType.Sport)
            .ToList();

        return mapper.Map<List<BikeGetDto>>(sportBikes);
    }

    /// <summary>
    /// A method that returns the top 5 bike models by rental duration
    /// </summary>
    public List<BikeModelGetDto> GetTopFiveModelsByRentDuration()
    {
        var allRents = rentRepository.ReadAll();
        var allBikes = bikeRepository.ReadAll();
        var allModels = bikeModelRepository.ReadAll();

        var bikeDict = allBikes.ToDictionary(b => b.Id);
        var modelDict = allModels.ToDictionary(m => m.Id);

        foreach (var bike in allBikes)
        {
            if (modelDict.TryGetValue(bike.ModelId, out var model))
                bike.Model = model;
        }

        foreach (var rent in allRents)
        {
            if (bikeDict.TryGetValue(rent.BikeId, out var bike))
                rent.Bike = bike;
        }

        var topModels = allRents
            .Where(rent => rent.Bike != null && rent.Bike.Model != null)
            .GroupBy(rent => rent.Bike!.Model)
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
        var allRents = rentRepository.ReadAll();
        var allBikes = bikeRepository.ReadAll();
        var allModels = bikeModelRepository.ReadAll();

        var bikeDict = allBikes.ToDictionary(b => b.Id);
        var modelDict = allModels.ToDictionary(m => m.Id);

        foreach (var bike in allBikes)
        {
            if (modelDict.TryGetValue(bike.ModelId, out var model))
                bike.Model = model;
        }

        foreach (var rent in allRents)
        {
            if (bikeDict.TryGetValue(rent.BikeId, out var bike))
                rent.Bike = bike;
        }

        var topModels = allRents
            .Where(rent => rent.Bike != null && rent.Bike.Model != null)
            .GroupBy(rent => rent.Bike!.Model)
            .Select(group => new
            {
                Model = group.Key,
                TotalProfit = group.Sum(rent => rent.RentalDuration * rent.Bike!.Model!.RentPrice)
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
        var allRents = rentRepository.ReadAll();
        var allBikes = bikeRepository.ReadAll();
        var allModels = bikeModelRepository.ReadAll();

        var bikeDict = allBikes.ToDictionary(b => b.Id);
        var modelDict = allModels.ToDictionary(m => m.Id);

        foreach (var bike in allBikes)
        {
            if (modelDict.TryGetValue(bike.ModelId, out var model))
                bike.Model = model;
        }

        foreach (var rent in allRents)
        {
            if (bikeDict.TryGetValue(rent.BikeId, out var bike))
                rent.Bike = bike;
        }

        return allRents
            .Where(rent => rent.Bike != null && rent.Bike.Model != null)
            .GroupBy(rent => rent.Bike!.Model!.Type)
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
        var allRents = rentRepository.ReadAll();
        var allRenters = renterRepository.ReadAll();

        var renterDict = allRenters.ToDictionary(r => r.Id);
        foreach (var rent in allRents)
        {
            if (renterDict.TryGetValue(rent.RenterId, out var renter))
                rent.Renter = renter;
        }

        var topRenters = allRents
            .Where(rent => rent.Renter != null)
            .GroupBy(rent => rent.Renter!.Id)
            .Select(group => new
            {
                RenterId = group.Key,
                TotalRentals = group.Count()
            })
            .OrderByDescending(r => r.TotalRentals)
            .Take(3)
            .Join(allRenters,
                  x => x.RenterId,
                  renter => renter.Id,
                  (x, renter) => renter)
            .ToList();

        return mapper.Map<List<RenterGetDto>>(topRenters);
    }
}
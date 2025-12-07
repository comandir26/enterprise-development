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
    IRepository<Renter, int> renterRepository) : IAnalyticsService
{
    /// <summary>
    /// A method that returns information about all sports bikes
    /// </summary>
    public List<BikeDto> GetSportBikes()
    {
        return [.. bikeRepository.ReadAll()
            .Where(bike => bike.Model.Type == BikeType.Sport)
            .Select(bike => new BikeDto
            {
                SerialNumber = bike.SerialNumber,
                Color = bike.Color,
                ModelId = bike.Model.Id
            })];
    }

    /// <summary>
    /// A method that returns the top 5 bike models by rental duration
    /// </summary>
    public List<BikeModelDto> GetTopFiveModelsByRentDuration()
    {
        var rents = rentRepository.ReadAll();
        var models = bikeModelRepository.ReadAll();

        return [.. rents
            .GroupBy(rent => rent.Bike.Model.Id)
            .Select(group => new
            {
                ModelId = group.Key,
                TotalDuration = group.Sum(rent => rent.RentalDuration)
            })
            .OrderByDescending(x => x.TotalDuration)
            .Take(5)
            .Join(models,
                  x => x.ModelId,
                  model => model.Id,
                  (x, model) => new BikeModelDto
                  {
                      Type = model.Type,
                      WheelSize = model.WheelSize,
                      MaxPassengerWeight = model.MaxPassengerWeight,
                      Weight = model.Weight,
                      BrakeType = model.BrakeType,
                      Year = model.Year,
                      RentPrice = model.RentPrice
                  })];
    }

    /// <summary>
    /// A method that returns the top 5 bike models in terms of rental income
    /// </summary>
    public List<BikeModelDto> GetTopFiveModelsByProfit()
    {
        var rents = rentRepository.ReadAll();
        var models = bikeModelRepository.ReadAll();

        return [.. rents
            .GroupBy(rent => rent.Bike.Model.Id)
            .Select(group => new
            {
                ModelId = group.Key,
                TotalProfit = group.Sum(rent => rent.RentalDuration * rent.Bike.Model.RentPrice)
            })
            .OrderByDescending(x => x.TotalProfit)
            .Take(5)
            .Join(models,
                  x => x.ModelId,
                  model => model.Id,
                  (x, model) => new BikeModelDto
                  {
                      Type = model.Type,
                      WheelSize = model.WheelSize,
                      MaxPassengerWeight = model.MaxPassengerWeight,
                      Weight = model.Weight,
                      BrakeType = model.BrakeType,
                      Year = model.Year,
                      RentPrice = model.RentPrice
                  })];
    }

    /// <summary>
    /// A method that returns information about the minimum, maximum, and average bike rental time.
    /// </summary>
    public RentalDurationStatsDto GetRentalDurationStats()
    {
        List<int> durations = [.. rentRepository.ReadAll()
            .Select(rent => rent.RentalDuration)];

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
    public List<RenterDto> GetTopThreeRenters()
    {
        var renters = renterRepository.ReadAll();

        return [.. rentRepository.ReadAll()
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
                  (x, renter) => new RenterDto
                  {
                      FullName = renter.FullName,
                      Number = renter.Number
                  })];
    }
}
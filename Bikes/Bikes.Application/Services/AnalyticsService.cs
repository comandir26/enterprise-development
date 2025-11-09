using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IRepository<Bike, int> _bikeRepository;
    private readonly IRepository<BikeModel, int> _bikeModelRepository;
    private readonly IRepository<Rent, int> _rentRepository;
    private readonly IRepository<Renter, int> _renterRepository;

    public AnalyticsService(
        IRepository<Bike, int> bikeRepository,
        IRepository<BikeModel, int> bikeModelRepository,
        IRepository<Rent, int> rentRepository,
        IRepository<Renter, int> renterRepository)
    {
        _bikeRepository = bikeRepository;
        _bikeModelRepository = bikeModelRepository;
        _rentRepository = rentRepository;
        _renterRepository = renterRepository;
    }

    public List<Bike> GetSportBikes()
    {
        return _bikeRepository.ReadAll()
            .Where(bike => bike.Model.Type == BikeType.Sport)
            .ToList();
    }

    public List<BikeModel> GetTopFiveModelsByRentDuration()
    {
        var rents = _rentRepository.ReadAll();
        var models = _bikeModelRepository.ReadAll();

        return rents
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
                  (x, model) => model)
            .ToList();
    }

    public List<BikeModel> GetTopFiveModelsByProfit()
    {
        var rents = _rentRepository.ReadAll();
        var models = _bikeModelRepository.ReadAll();

        return rents
            .GroupBy(rent => rent.Bike.Model.Id)
            .Select(group => new
            {
                ModelId = group.Key,
                TotalProfit = group.Sum(rent => rent.RentalDuration * rent.Bike.Model.RentPrice)
            })
            .OrderByDescending(x => x.TotalProfit)
            .Take(5)
            .Join(models, x => x.ModelId, model => model.Id, (x, model) => model)
            .ToList();
    }

    public (int min, int max, double avg) GetRentalDurationStats()
    {
        var durations = _rentRepository.ReadAll()
            .Select(rent => rent.RentalDuration)
            .ToList();

        return (durations.Min(), durations.Max(), durations.Average());
    }

    public Dictionary<BikeType, int> GetTotalRentalTimeByType()
    {
        return _rentRepository.ReadAll()
            .GroupBy(rent => rent.Bike.Model.Type)
            .ToDictionary(
                group => group.Key,
                group => group.Sum(rent => rent.RentalDuration)
            );
    }

    public List<Renter> GetTopThreeRenters()
    {
        var renters = _renterRepository.ReadAll();

        return _rentRepository.ReadAll()
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
    }
}
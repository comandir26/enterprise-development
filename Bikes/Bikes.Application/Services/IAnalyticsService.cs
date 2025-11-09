using Bikes.Domain.Models;

namespace Bikes.Application.Services;

public interface IAnalyticsService
{
    public List<Bike> GetSportBikes();
    public List<BikeModel> GetTopFiveModelsByRentDuration();
    public List<BikeModel> GetTopFiveModelsByProfit();
    public (int min, int max, double avg) GetRentalDurationStats();
    public Dictionary<BikeType, int> GetTotalRentalTimeByType();
    public List<Renter> GetTopThreeRenters();
}
using Bikes.Domain.Models;

namespace Bikes.Application.Services;

/// <summary>
/// Interface for the Analytics service class
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// A method that returns information about all sports bikes
    /// </summary>
    public List<Bike> GetSportBikes();

    /// <summary>
    /// A method that returns the top 5 bike models by rental duration
    /// </summary>
    public List<BikeModel> GetTopFiveModelsByRentDuration();

    /// <summary>
    /// A method that returns the top 5 bike models in terms of rental income
    /// </summary>
    public List<BikeModel> GetTopFiveModelsByProfit();

    /// <summary>
    /// A method that returns information about the minimum, maximum, and average bike rental time.
    /// </summary>
    public (int min, int max, double avg) GetRentalDurationStats();

    /// <summary>
    /// A method that returns the total rental time of each type of bike
    /// </summary>
    public Dictionary<BikeType, int> GetTotalRentalTimeByType();

    /// <summary>
    /// A method that returns information about the customers who have rented bicycles the most times.
    /// </summary>
    public List<Renter> GetTopThreeRenters();
}
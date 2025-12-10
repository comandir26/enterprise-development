using Bikes.Domain.Models;

namespace Bikes.Contracts.Dto;

/// <summary>
/// DTO get class for the Rent class
/// </summary>
public class RentGetDto
{
    /// <summary>
    /// Rent's unique id
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Rental start time
    /// </summary>
    public required DateTime RentalStartTime { get; set; }

    /// <summary>
    /// Rental duration
    /// </summary>
    public required int RentalDuration { get; set; }

    /// <summary>
    /// Renter
    /// </summary>
    public required int RenterId { get; set; }

    /// <summary>
    /// Bike
    /// </summary>
    public required int BikeId { get; set; }
}
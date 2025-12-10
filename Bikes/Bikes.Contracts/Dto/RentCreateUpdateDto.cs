namespace Bikes.Contracts.Dto;

/// <summary>
/// DTO create/update class for the Rent class
/// </summary>
public class RentCreateUpdateDto
{
    /// <summary>
    /// Rental start time
    /// </summary>
    public required DateTime RentalStartTime { get; set; }

    /// <summary>
    /// Rental duration
    /// </summary>
    public required int RentalDuration { get; set; }

    /// <summary>
    /// Renter's id
    /// </summary>
    public required int RenterId { get; set; }

    /// <summary>
    /// Bike's id
    /// </summary>
    public required int BikeId { get; set; }
}
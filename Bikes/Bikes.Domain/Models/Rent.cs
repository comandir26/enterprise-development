namespace Bikes.Domain.Models;

/// <summary>
/// A class describing a rent
/// </summary>
public class Rent
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
    public required Renter Renter { get; set; }

    /// <summary>
    /// Bike
    /// </summary>
    public required Bike Bike { get; set; }
}

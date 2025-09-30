namespace Bikes.Domain.Models;

/// <summary>
/// A class describing a renter
/// </summary>
public class Renter
{
    /// <summary>
    /// Renter's unique id
    /// </summary>
    public required int Id { get; set; }
    /// <summary>
    /// Renter's full name
    /// </summary>
    public required string FullName { get; set; }
    /// <summary>
    /// Renter's phone number
    /// </summary>
    public required string Number { get; set; }
}

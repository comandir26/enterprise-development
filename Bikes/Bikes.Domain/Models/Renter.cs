namespace Bikes.Domain.Models;
public class Renter
{
    /// <summary>
    /// The unique id for the renter
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

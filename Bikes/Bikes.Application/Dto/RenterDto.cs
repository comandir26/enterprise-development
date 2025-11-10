namespace Bikes.Application.Dto;

/// <summary>
/// DTO class for the Renter class
/// </summary>
public class RenterDto
{
    /// <summary>
    /// Renter's full name
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Renter's phone number
    /// </summary>
    public required string Number { get; set; }
}
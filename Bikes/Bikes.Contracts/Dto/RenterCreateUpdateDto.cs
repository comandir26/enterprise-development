namespace Bikes.Contracts.Dto;

/// <summary>
/// DTO create/update class for the Renter class
/// </summary>
public class RenterCreateUpdateDto
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
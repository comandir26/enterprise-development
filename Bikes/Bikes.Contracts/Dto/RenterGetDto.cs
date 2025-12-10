namespace Bikes.Contracts.Dto;

/// <summary>
/// DTO get class for the Renter class
/// </summary>
public class RenterGetDto
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
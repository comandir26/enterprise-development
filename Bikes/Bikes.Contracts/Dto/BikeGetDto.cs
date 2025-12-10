namespace Bikes.Contracts.Dto;

/// <summary>
/// DTO get class for the Bike class
/// </summary>
public class BikeGetDto
{
    /// <summary>
    /// Bike's unique id
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Bike's serial number
    /// </summary>
    public required string SerialNumber { get; set; }

    /// <summary>
    /// Bike's color
    /// </summary>
    public required string Color { get; set; }

    /// <summary>
    /// Bike's model
    /// </summary>
    public required int ModelId { get; set; }
}
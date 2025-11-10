namespace Bikes.Application.Dto;

/// <summary>
/// DTO class for the Bike class
/// </summary>
public class BikeDto
{
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

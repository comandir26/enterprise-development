namespace Bikes.Contracts.Dto;

/// <summary>
/// DTO class for rental duration statistics
/// </summary>
public class RentalDurationStatsDto
{
    /// <summary>
    /// Minimum rental duration
    /// </summary>
    public required int Min { get; set; }

    /// <summary>
    /// Maximum rental duration
    /// </summary>
    public required int Max { get; set; }

    /// <summary>
    /// Average rental duration
    /// </summary>
    public required double Average { get; set; }
}
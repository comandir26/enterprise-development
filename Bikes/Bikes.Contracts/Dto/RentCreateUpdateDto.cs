using System.ComponentModel.DataAnnotations;

namespace Bikes.Contracts.Dto;

/// <summary>
/// DTO create/update class for the Rent class
/// </summary>
public class RentCreateUpdateDto
{
    /// <summary>
    /// Rental start time
    /// </summary>
    [Required(ErrorMessage = "Rental start time is required")]
    [DataType(DataType.DateTime, ErrorMessage = "Invalid date time format")]
    public required DateTime RentalStartTime { get; set; }

    /// <summary>
    /// Rental duration (in hours)
    /// </summary>
    [Required(ErrorMessage = "Rental duration is required")]
    [Range(1, 24, ErrorMessage = "Rental duration must be between 1 and 24 hours")]
    public required int RentalDuration { get; set; }

    /// <summary>
    /// Renter's id
    /// </summary>
    [Required(ErrorMessage = "Renter ID is required")]
    public required int RenterId { get; set; }

    /// <summary>
    /// Bike's id
    /// </summary>
    [Required(ErrorMessage = "Bike ID is required")]
    public required int BikeId { get; set; }
}
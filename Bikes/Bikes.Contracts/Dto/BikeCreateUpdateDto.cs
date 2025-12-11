using System.ComponentModel.DataAnnotations;

namespace Bikes.Contracts.Dto;

/// <summary>
/// DTO create/update class for the Bike class
/// </summary>
public class BikeCreateUpdateDto
{
    /// <summary>
    /// Bike's serial number
    /// </summary>
    [Required(ErrorMessage = "Serial number is required")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Serial number must be between 5 and 50 characters")]
    [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Serial number can only contain uppercase letters and numbers")]
    public required string SerialNumber { get; set; }

    /// <summary>
    /// Bike's color
    /// </summary>
    [Required(ErrorMessage = "Color is required")]
    [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ]+$", ErrorMessage = "Color can only contain letters")]
    public required string Color { get; set; }

    /// <summary>
    /// Bike's model ID
    /// </summary>
    [Required(ErrorMessage = "Model ID is required")]
    public required int ModelId { get; set; }
}
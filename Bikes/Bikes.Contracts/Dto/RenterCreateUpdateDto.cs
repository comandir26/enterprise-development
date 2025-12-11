using System.ComponentModel.DataAnnotations;

namespace Bikes.Contracts.Dto;

/// <summary>
/// DTO create/update class for the Renter class
/// </summary>
public class RenterCreateUpdateDto
{
    /// <summary>
    /// Renter's full name
    /// </summary>
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Full name must be between 5 and 100 characters")]
    [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s\-]+$", ErrorMessage = "Full name can only contain letters, spaces and hyphens")]
    public required string FullName { get; set; }

    /// <summary>
    /// Renter's phone number
    /// </summary>
    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^\+7\s\(\d{3}\)\s\d{3}-\d{2}-\d{2}$",
        ErrorMessage = "Phone number must be in format: +7 (XXX) XXX-XX-XX")]
    public required string Number { get; set; }
}
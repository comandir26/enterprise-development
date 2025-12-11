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
    public required string FullName { get; set; }

    /// <summary>
    /// Renter's phone number
    /// </summary>
    [Required(ErrorMessage = "Phone number is required")]
    public required string Number { get; set; }
}
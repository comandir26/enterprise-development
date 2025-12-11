using Bikes.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Bikes.Contracts.Dto;

/// <summary>
/// DTO create/update class for the BikeModel class
/// </summary>
public class BikeModelCreateUpdateDto
{
    /// <summary>
    /// Model's type
    /// </summary>
    [Required(ErrorMessage = "Type is required")]
    public required BikeType Type { get; set; }

    /// <summary>
    /// Model's size of wheel
    /// </summary>
    [Required(ErrorMessage = "Wheel size is required")]
    [Range(12, 36, ErrorMessage = "Wheel size must be between 12 and 36 inches")]
    public required int WheelSize { get; set; }

    /// <summary>
    /// Maximum allowable passenger weight
    /// </summary>
    [Required(ErrorMessage = "Maximum passenger weight is required")]
    [Range(25, 120, ErrorMessage = "Maximum passenger weight must be between 25 and 120 kg")]
    public required int MaxPassengerWeight { get; set; }

    /// <summary>
    /// Model's weight
    /// </summary>
    [Required(ErrorMessage = "Weight is required")]
    [Range(5, 30, ErrorMessage = "Weight must be between 5 and 30 kg")]
    public required int Weight { get; set; }

    /// <summary>
    /// Model's type of brake
    /// </summary>
    [Required(ErrorMessage = "Brake type is required")]
    public required string BrakeType { get; set; }

    /// <summary>
    /// Model's production year
    /// </summary>
    [Required(ErrorMessage = "Year is required")]
    [Range(2010, 2025, ErrorMessage = "Year must be between 2010 and current year")]
    public required int Year { get; set; }

    /// <summary>
    /// The price of an hour of rent
    /// </summary>
    [Required(ErrorMessage = "Rent price is required")]
    [Range(1, 1000, ErrorMessage = "Rent price must be between 1 and 1000")]
    public required int RentPrice { get; set; }
}
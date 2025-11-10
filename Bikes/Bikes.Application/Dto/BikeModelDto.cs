using Bikes.Domain.Models;

namespace Bikes.Application.Dto;

/// <summary>
/// DTO class for the BikeModel class
/// </summary>
public class BikeModelDto
{
    /// <summary>
    /// Model's type
    /// </summary>
    public required BikeType Type { get; set; }

    /// <summary>
    /// Model's size of wheel
    /// </summary>
    public required int WheelSize { get; set; }

    /// <summary>
    /// Maximum allowable passenger weight
    /// </summary>
    public required int MaxPassengerWeight { get; set; }

    /// <summary>
    /// Model's weight
    /// </summary>
    public required int Weight { get; set; }

    /// <summary>
    /// Model's type of brake
    /// </summary>
    public required string BrakeType { get; set; }

    /// <summary>
    /// Model's production year
    /// </summary>
    public required string Year { get; set; }

    /// <summary>
    /// The price of an hour of rent
    /// </summary>
    public required int RentPrice { get; set; }
}
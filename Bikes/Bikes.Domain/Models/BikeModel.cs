namespace Bikes.Domain.Models;

/// <summary>
/// A class describing a bike's model
/// </summary>
public class BikeModel
{
    /// <summary>
    /// Model's unique id
    /// </summary>
    public required int Id { get; set; }

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

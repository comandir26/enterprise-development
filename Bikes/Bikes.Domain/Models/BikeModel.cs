namespace Bikes.Domain.Models;

public class BikeModel
{
    public required int Id { get; set; }
    public required BikeType Type { get; set; }
    public required int WheelSize { get; set; }
    public required int MaxPassengerWeight { get; set; }
    public required int Weight { get; set; }
    public required string BrakeType { get; set; }
    public required string Year { get; set; }
    public required int RentPrice { get; set; }
}

namespace Bikes.Application.Dto;

public class RentDto
{
    public required DateTime RentalStartTime { get; set; }
    public required int RentalDuration { get; set; }
    public required int RenterId { get; set; }
    public required int BikeId { get; set; }
}
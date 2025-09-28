namespace Bikes.Domain.Models;

public class Rent
{
    public required int Id { get; set; }
    public required DateTime RentalStartTime { get; set; }
    public required int RentalDuration { get; set; }
    public required Renter Renter { get; set; }
    public required Bike Bike { get; set; }
}

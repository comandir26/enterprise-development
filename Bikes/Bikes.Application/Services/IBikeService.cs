using Bikes.Application.Dto;
using Bikes.Domain.Models;

namespace Bikes.Application.Services;

public interface IBikeService
{
    public int CreateBike(BikeDto bikeDto);
    public List<Bike> GetAllBikes();
    public Bike? GetBikeById(int id);
    public Bike? UpdateBike(int id, BikeDto bikeDto);
    public bool DeleteBike(int id);
}
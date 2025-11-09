using Bikes.Application.Dto;
using Bikes.Domain.Models;

namespace Bikes.Application.Services;

public interface IRentService
{
    public int CreateRent(RentDto rentDto);
    public List<Rent> GetAllRents();
    public Rent? GetRentById(int id);
    public Rent? UpdateRent(int id, RentDto rentDto);
    public bool DeleteRent(int id);
}
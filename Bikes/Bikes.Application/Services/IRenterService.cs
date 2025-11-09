using Bikes.Application.Dto;
using Bikes.Domain.Models;

namespace Bikes.Application.Services;

public interface IRenterService
{
    public int CreateRenter(RenterDto renterDto);
    public List<Renter> GetAllRenters();
    public Renter? GetRenterById(int id);
    public Renter? UpdateRenter(int id, RenterDto renterDto);
    public bool DeleteRenter(int id);
}
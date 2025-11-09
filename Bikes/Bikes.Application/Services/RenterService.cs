using Bikes.Application.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

public class RenterService : IRenterService
{
    private readonly IRepository<Renter, int> _renterRepository;

    public RenterService(IRepository<Renter, int> renterRepository)
    {
        _renterRepository = renterRepository;
    }

    private static Renter MapToDomain(RenterDto dto, int id = 0)
    {
        return new Renter
        {
            Id = id,
            FullName = dto.FullName,
            Number = dto.Number
        };
    }

    public int CreateRenter(RenterDto renterDto)
    {
        var renter = MapToDomain(renterDto);
        return _renterRepository.Create(renter);
    }

    public List<Renter> GetAllRenters() => _renterRepository.ReadAll();

    public Renter? GetRenterById(int id) => _renterRepository.Read(id);

    public Renter? UpdateRenter(int id, RenterDto renterDto)
    {
        var existingRenter = _renterRepository.Read(id);
        if (existingRenter == null) return null;

        var updatedRenter = MapToDomain(renterDto, id);
        return _renterRepository.Update(id, updatedRenter);
    }

    public bool DeleteRenter(int id) => _renterRepository.Delete(id);
}

using Bikes.Domain.Models;
using Bikes.Domain.Repositories;
using Bikes.Infrastructure.InMemory.Seeders;

namespace Bikes.Infrastructure.InMemory.Repositories;

public class InMemoryRentRepository : IRepository<Rent, int>
{
    private readonly List<Rent> _items = [];

    private int _currentId;

    public InMemoryRentRepository()
    {
        _items = InMemorySeeder.GetRents();
        _currentId = _items.Count > 0 ? _items.Count : 0;
    }

    public int Create(Rent entity)
    {
        entity.Id = ++_currentId;
        _items.Add(entity);
        return entity.Id;
    }

    public List<Rent> ReadAll()
    {
        return _items;
    }

    public Rent? Read(int id)
    {
        return _items.FirstOrDefault(b => b.Id == id);
    }

    public Rent? Update(int id, Rent entity)
    {
        var exsitingEntity = Read(id);
        if (exsitingEntity == null) return null;

        exsitingEntity.RentalStartTime = entity.RentalStartTime;
        exsitingEntity.RentalDuration = entity.RentalDuration;
        exsitingEntity.Renter = entity.Renter;
        exsitingEntity.Bike = entity.Bike;

        return exsitingEntity;
    }

    public bool Delete(int id)
    {
        var exsitingEntity = Read(id);
        if (exsitingEntity == null) return false;

        _items.Remove(exsitingEntity);
        return true;
    }
}

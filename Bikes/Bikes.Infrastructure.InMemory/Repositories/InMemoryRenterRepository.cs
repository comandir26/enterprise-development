using Bikes.Domain.Models;
using Bikes.Domain.Repositories;
using Bikes.Infrastructure.InMemory.Seeders;

namespace Bikes.Infrastructure.InMemory.Repositories;

public class InMemoryRenterRepository : IRepository<Renter, int>
{
    private readonly List<Renter> _items = [];

    private int _currentId;

    public InMemoryRenterRepository()
    {
        _items = InMemorySeeder.GetRenters();
        _currentId = _items.Count > 0 ? _items.Count : 0;
    }

    public int Create(Renter entity)
    {
        entity.Id = ++_currentId;
        _items.Add(entity);
        return entity.Id;
    }

    public List<Renter> ReadAll()
    {
        return _items;
    }

    public Renter? Read(int id)
    {
        return _items.FirstOrDefault(b => b.Id == id);
    }

    public Renter? Update(int id, Renter entity)
    {
        var exsitingEntity = Read(id);
        if (exsitingEntity == null) return null;

        exsitingEntity.FullName = entity.FullName;
        exsitingEntity.Number = entity.Number;

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

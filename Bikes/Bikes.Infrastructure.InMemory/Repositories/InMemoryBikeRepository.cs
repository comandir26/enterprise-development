using Bikes.Domain.Models;
using Bikes.Domain.Repositories;
using Bikes.Infrastructure.InMemory.Seeders;

namespace Bikes.Infrastructure.InMemory.Repositories;

public class InMemoryBikeRepository : IRepository<Bike, int>
{
    private readonly List<Bike> _items = [];

    private int _currentId;

    public InMemoryBikeRepository()
    {
        _items = InMemorySeeder.GetBikes();
        _currentId = _items.Count > 0 ? _items.Count : 0;
    }

    public int Create(Bike entity)
    {
        entity.Id  = ++_currentId;
        _items.Add(entity);
        return entity.Id;
    }

    public List<Bike> ReadAll()
    {
        return _items;
    }

    public Bike? Read(int id)
    {
        return _items.FirstOrDefault(b => b.Id == id); 
    }

    public Bike? Update(int id, Bike entity)
    {
        var exsitingEntity = Read(id);
        if (exsitingEntity == null) return null;

        exsitingEntity.SerialNumber = entity.SerialNumber;
        exsitingEntity.Color = entity.Color;
        exsitingEntity.Model = entity.Model;

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

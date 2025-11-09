using Bikes.Domain.Models;
using Bikes.Domain.Repositories;
using Bikes.Infrastructure.InMemory.Seeders;

namespace Bikes.Infrastructure.InMemory.Repositories;

public class InMemoryBikeModelRepository : IRepository<BikeModel, int>
{
    private readonly List<BikeModel> _items = [];

    private int _currentId;

    public InMemoryBikeModelRepository()
    {
        _items = InMemorySeeder.GetBikeModels();
        _currentId = _items.Count > 0 ? _items.Count : 0;
    }

    public int Create(BikeModel entity)
    {
        entity.Id = ++_currentId;
        _items.Add(entity);
        return entity.Id;
    }

    public List<BikeModel> ReadAll()
    {
        return _items;
    }

    public BikeModel? Read(int id)
    {
        return _items.FirstOrDefault(b => b.Id == id);
    }

    public BikeModel? Update(int id, BikeModel entity)
    {
        var exsitingEntity = Read(id);
        if (exsitingEntity == null) return null;

        exsitingEntity.Type = entity.Type;
        exsitingEntity.WheelSize = entity.WheelSize;
        exsitingEntity.MaxPassengerWeight = entity.MaxPassengerWeight;
        exsitingEntity.Weight = entity.Weight;
        exsitingEntity.BrakeType = entity.BrakeType;
        exsitingEntity.Year = entity.Year;
        exsitingEntity.RentPrice = entity.RentPrice;

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

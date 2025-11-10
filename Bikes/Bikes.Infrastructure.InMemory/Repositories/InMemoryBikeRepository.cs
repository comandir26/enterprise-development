using Bikes.Domain.Models;
using Bikes.Domain.Repositories;
using Bikes.Infrastructure.InMemory.Seeders;

namespace Bikes.Infrastructure.InMemory.Repositories;

/// <summary>
/// Implementing a repository for the Bike class 
/// </summary>
public class InMemoryBikeRepository : IRepository<Bike, int>
{
    private readonly List<Bike> _items = [];

    private int _currentId;

    /// <summary>
    /// A constructor that uses data from InMemorySeeder
    /// </summary>
    public InMemoryBikeRepository()
    {
        _items = InMemorySeeder.GetBikes();
        _currentId = _items.Count > 0 ? _items.Count : 0;
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="entity">Object</param>
    /// <returns>ID of the created object</returns>
    public int Create(Bike entity)
    {
        entity.Id  = ++_currentId;
        _items.Add(entity);
        return entity.Id;
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<Bike> ReadAll()
    {
        return _items;
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Object if exist</returns>
    public Bike? Read(int id)
    {
        return _items.FirstOrDefault(b => b.Id == id); 
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Object</param>
    /// <returns>Object if exist</returns>
    public Bike? Update(int id, Bike entity)
    {
        var exsitingEntity = Read(id);
        if (exsitingEntity == null) return null;

        exsitingEntity.SerialNumber = entity.SerialNumber;
        exsitingEntity.Color = entity.Color;
        exsitingEntity.Model = entity.Model;

        return exsitingEntity;
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool Delete(int id)
    {
        var exsitingEntity = Read(id);
        if (exsitingEntity == null) return false;

        _items.Remove(exsitingEntity);
        return true;
    }
}

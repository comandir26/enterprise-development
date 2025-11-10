using Bikes.Domain.Models;
using Bikes.Domain.Repositories;
using Bikes.Infrastructure.InMemory.Seeders;

namespace Bikes.Infrastructure.InMemory.Repositories;

/// <summary>
/// Implementing a repository for the Rent class 
/// </summary>
public class InMemoryRentRepository : IRepository<Rent, int>
{
    private readonly List<Rent> _items = [];

    private int _currentId;

    /// <summary>
    /// A constructor that uses data from InMemorySeeder
    /// </summary>
    public InMemoryRentRepository()
    {
        _items = InMemorySeeder.GetRents();
        _currentId = _items.Count > 0 ? _items.Count : 0;
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="entity">Object</param>
    /// <returns>ID of the created object</returns>
    public int Create(Rent entity)
    {
        entity.Id = ++_currentId;
        _items.Add(entity);
        return entity.Id;
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<Rent> ReadAll()
    {
        return _items;
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Object if exist</returns>
    public Rent? Read(int id)
    {
        return _items.FirstOrDefault(b => b.Id == id);
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Object</param>
    /// <returns>Object if exist</returns>
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

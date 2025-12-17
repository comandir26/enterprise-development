using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Infrastructure.MongoDb.Repositories;

/// <summary>
/// A repository for working with bikes in MongoDB
/// </summary>
public class MongoBikeRepository(
    BikesDbContext context) : IRepository<Bike, int>
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="entity">Object</param>
    /// <returns>ID of the created object</returns>
    public int Create(Bike entity)
    {
        if (entity.Id == 0)
        {
            var lastId = context.Bikes
                .OrderByDescending(b => b.Id)
                .Select(b => b.Id)
                .FirstOrDefault();

            entity.Id = lastId + 1;
        }

        context.Bikes.Add(entity);
        context.SaveChanges();
        return entity.Id;
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<Bike> ReadAll()
    {
        return [.. context.Bikes];
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Object if exist</returns>
    public Bike? Read(int id)
    {
        return context.Bikes.FirstOrDefault(b => b.Id == id);
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Object</param>
    /// <returns>Object if exist</returns>
    public Bike? Update(int id, Bike entity)
    {
        var existingBike = context.Bikes.FirstOrDefault(b => b.Id == id);
        if (existingBike == null) return null;

        existingBike.SerialNumber = entity.SerialNumber;
        existingBike.Color = entity.Color;
        existingBike.ModelId = entity.ModelId;

        context.SaveChanges();
        return existingBike;
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool Delete(int id)
    {
        var bike = context.Bikes.Find(id);
        if (bike == null) return false;

        context.Bikes.Remove(bike);
        context.SaveChanges();
        return true;
    }
}
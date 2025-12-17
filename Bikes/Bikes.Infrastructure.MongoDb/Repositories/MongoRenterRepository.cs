using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Infrastructure.MongoDb.Repositories;

/// <summary>
/// A repository for working with renters in MongoDB
/// </summary>
public class MongoRenterRepository(
    BikesDbContext context) : IRepository<Renter, int>
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="entity">Object</param>
    /// <returns>ID of the created object</returns>
    public int Create(Renter entity)
    {
        if (entity.Id == 0)
        {
            var lastId = context.Renters
                .OrderByDescending(b => b.Id)
                .Select(b => b.Id)
                .FirstOrDefault();

            entity.Id = lastId + 1;
        }

        context.Renters.Add(entity);
        context.SaveChanges();
        return entity.Id;
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<Renter> ReadAll()
    {
        return [.. context.Renters];
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Object if exist</returns>
    public Renter? Read(int id)
    {
        return context.Renters.Find(id);
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Object</param>
    /// <returns>Object if exist</returns>
    public Renter? Update(int id, Renter entity)
    {
        var existingRenter = context.Renters.Find(id);
        if (existingRenter == null) return null;

        context.Entry(existingRenter).CurrentValues.SetValues(entity);
        existingRenter.Id = id;

        context.SaveChanges();
        return existingRenter;
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool Delete(int id)
    {
        var renter = context.Renters.Find(id);
        if (renter == null) return false;

        context.Renters.Remove(renter);
        context.SaveChanges();
        return true;
    }
}
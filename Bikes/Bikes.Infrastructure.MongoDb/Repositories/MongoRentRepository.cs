using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Infrastructure.MongoDb.Repositories;

/// <summary>
/// A repository for working with rents in MongoDB
/// </summary>
public class MongoRentRepository(
    BikesDbContext context) : IRepository<Rent, int>
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="entity">Object</param>
    /// <returns>ID of the created object</returns>
    public int Create(Rent entity)
    {
        if (entity.Id == 0)
        {
            var lastId = context.Rents
                .OrderByDescending(b => b.Id)
                .Select(b => b.Id)
                .FirstOrDefault();

            entity.Id = lastId + 1;
        }

        context.Rents.Add(entity);
        context.SaveChanges();
        return entity.Id;
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<Rent> ReadAll()
    {
        return [.. context.Rents];
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Object if exist</returns>
    public Rent? Read(int id)
    {
        return context.Rents.FirstOrDefault(r => r.Id == id);
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Object</param>
    /// <returns>Object if exist</returns>
    public Rent? Update(int id, Rent entity)
    {
        var existingRent = context.Rents.FirstOrDefault(r => r.Id == id);
        if (existingRent == null) return null;

        existingRent.RentalStartTime = entity.RentalStartTime;
        existingRent.RentalDuration = entity.RentalDuration;
        existingRent.BikeId = entity.BikeId;
        existingRent.RenterId = entity.RenterId;

        context.SaveChanges();
        return existingRent;
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool Delete(int id)
    {
        var rent = context.Rents.Find(id);
        if (rent == null) return false;

        context.Rents.Remove(rent);
        context.SaveChanges();
        return true;
    }
}
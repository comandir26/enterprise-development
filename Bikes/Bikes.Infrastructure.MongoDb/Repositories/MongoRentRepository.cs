using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Infrastructure.MongoDb.Repositories;

/// <summary>
/// A repository for working with rents in MongoDB
/// </summary>
public class MongoRentRepository : IRepository<Rent, int>
{
    private readonly BikesDbContext _context;

    public MongoRentRepository(BikesDbContext context) 
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="entity">Object</param>
    /// <returns>ID of the created object</returns>
    public int Create(Rent entity)
    {
        if (entity.Id == 0)
        {
            var lastId = _context.Rents
                .OrderByDescending(b => b.Id)
                .Select(b => b.Id)
                .FirstOrDefault();

            entity.Id = lastId + 1;
        }

        _context.Rents.Add(entity);
        _context.SaveChanges();
        return entity.Id;
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<Rent> ReadAll()
    {
        return _context.Rents.ToList();
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Object if exist</returns>
    public Rent? Read(int id)
    {
        return _context.Rents.FirstOrDefault(r => r.Id == id);
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Object</param>
    /// <returns>Object if exist</returns>
    public Rent? Update(int id, Rent entity)
    {
        var existingRent = _context.Rents.FirstOrDefault(r => r.Id == id);
        if (existingRent == null) return null;

        existingRent.RentalStartTime = entity.RentalStartTime;
        existingRent.RentalDuration = entity.RentalDuration;
        existingRent.BikeId = entity.BikeId;
        existingRent.RenterId = entity.RenterId;

        _context.SaveChanges();
        return existingRent;
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool Delete(int id)
    {
        var rent = _context.Rents.Find(id);
        if (rent == null) return false;

        _context.Rents.Remove(rent);
        _context.SaveChanges();
        return true;
    }
}
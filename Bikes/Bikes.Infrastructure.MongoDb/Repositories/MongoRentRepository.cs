using MongoDB.Driver;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Infrastructure.MongoDb.Repositories;

/// <summary>
/// Repository for working with rents in MongoDB
/// </summary>
public class MongoRentRepository : IRepository<Rent, int>
{
    private readonly IMongoCollection<Rent> _collection;
    private readonly MongoDbContext _context;

    public MongoRentRepository(MongoDbContext context)
    {
        _context = context;
        _collection = context.Rents;

        var indexKeysDefinition = Builders<Rent>.IndexKeys.Ascending(r => r.Id);
        _collection.Indexes.CreateOne(new CreateIndexModel<Rent>(indexKeysDefinition));
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="entity">Object</param>
    /// <returns>ID of the created object</returns>
    public int Create(Rent entity)
    {
        if (entity.Bike != null && entity.Bike.Id > 0)
        {
            entity.Bike = _context.Bikes.Find(b => b.Id == entity.Bike.Id).FirstOrDefault();
        }

        if (entity.Renter != null && entity.Renter.Id > 0)
        {
            entity.Renter = _context.Renters.Find(r => r.Id == entity.Renter.Id).FirstOrDefault();
        }

        var maxId = _collection.Find(_ => true)
            .SortByDescending(r => r.Id)
            .Limit(1)
            .FirstOrDefault()?.Id ?? 0;

        entity.Id = maxId + 1;
        _collection.InsertOne(entity);
        return entity.Id;
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<Rent> ReadAll()
    {

        var rents = _collection.Find(_ => true).ToList();

        foreach (var rent in rents)
        {
            LoadRelatedData(rent);
        }

        return rents;
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Object if exist</returns>
    public Rent? Read(int id)
    {
        var rent = _collection.Find(r => r.Id == id).FirstOrDefault();
        if (rent != null)
        {
            LoadRelatedData(rent);
        }
        return rent;
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Object</param>
    /// <returns>Object if exist</returns>
    public Rent? Update(int id, Rent entity)
    {
        entity.Id = id;

        LoadRelatedData(entity);

        var result = _collection.ReplaceOne(r => r.Id == id, entity);
        return result.ModifiedCount > 0 ? entity : null;
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool Delete(int id)
    {
        var result = _collection.DeleteOne(r => r.Id == id);
        return result.DeletedCount > 0;
    }

    /// <summary>
    /// Load related data
    /// </summary>
    /// <param name="rent"></param>
    private void LoadRelatedData(Rent rent)
    {
        if (rent.Bike != null && rent.Bike.Id > 0)
        {
            rent.Bike = _context.Bikes.Find(b => b.Id == rent.Bike.Id).FirstOrDefault();
        }

        if (rent.Renter != null && rent.Renter.Id > 0)
        {
            rent.Renter = _context.Renters.Find(r => r.Id == rent.Renter.Id).FirstOrDefault();
        }
    }
}
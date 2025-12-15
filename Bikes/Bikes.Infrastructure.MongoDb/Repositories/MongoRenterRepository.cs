using MongoDB.Driver;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Infrastructure.MongoDb.Repositories;

/// <summary>
/// Repository for working with renters in MongoDB
/// </summary>
public class MongoRenterRepository : IRepository<Renter, int>
{
    private readonly IMongoCollection<Renter> _collection;

    public MongoRenterRepository(MongoDbContext context)
    {
        _collection = context.Renters;

        var indexKeysDefinition = Builders<Renter>.IndexKeys.Ascending(r => r.Id);
        _collection.Indexes.CreateOne(new CreateIndexModel<Renter>(indexKeysDefinition));
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="entity">Object</param>
    /// <returns>ID of the created object</returns>
    public int Create(Renter entity)
    {
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
    public List<Renter> ReadAll()
    {
        return _collection.Find(_ => true).ToList();
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Object if exist</returns>
    public Renter? Read(int id)
    {
        return _collection.Find(r => r.Id == id).FirstOrDefault();
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Object</param>
    /// <returns>Object if exist</returns>
    public Renter? Update(int id, Renter entity)
    {
        entity.Id = id;
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
}
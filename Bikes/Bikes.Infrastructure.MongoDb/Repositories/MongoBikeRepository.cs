using MongoDB.Driver;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Infrastructure.MongoDb.Repositories;

/// <summary>
/// Repository for working with bikes in MongoDB
/// </summary>
public class MongoBikeRepository : IRepository<Bike, int>
{
    private readonly IMongoCollection<Bike> _collection;

    public MongoBikeRepository(MongoDbContext context)
    {
        _collection = context.Bikes;

        var indexKeysDefinition = Builders<Bike>.IndexKeys.Ascending(b => b.Id);
        _collection.Indexes.CreateOne(new CreateIndexModel<Bike>(indexKeysDefinition));
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="entity">Object</param>
    /// <returns>ID of the created object</returns>
    public int Create(Bike entity)
    {
        var maxId = _collection.Find(_ => true)
            .SortByDescending(b => b.Id)
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
    public List<Bike> ReadAll()
    {
        return _collection.Find(_ => true).ToList();
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Object if exist</returns>
    public Bike? Read(int id)
    {
        return _collection.Find(b => b.Id == id).FirstOrDefault();
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Object</param>
    /// <returns>Object if exist</returns>
    public Bike? Update(int id, Bike entity)
    {
        entity.Id = id; 
        var result = _collection.ReplaceOne(b => b.Id == id, entity);
        return result.ModifiedCount > 0 ? entity : null;
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool Delete(int id)
    {
        var result = _collection.DeleteOne(b => b.Id == id);
        return result.DeletedCount > 0;
    }
}
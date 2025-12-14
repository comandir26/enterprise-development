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

    public List<Bike> ReadAll()
    {
        return _collection.Find(_ => true).ToList();
    }

    public Bike? Read(int id)
    {
        return _collection.Find(b => b.Id == id).FirstOrDefault();
    }

    public Bike? Update(int id, Bike entity)
    {
        entity.Id = id; 
        var result = _collection.ReplaceOne(b => b.Id == id, entity);
        return result.ModifiedCount > 0 ? entity : null;
    }

    public bool Delete(int id)
    {
        var result = _collection.DeleteOne(b => b.Id == id);
        return result.DeletedCount > 0;
    }
}
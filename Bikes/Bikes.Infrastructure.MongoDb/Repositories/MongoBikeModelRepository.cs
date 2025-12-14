using MongoDB.Driver;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Infrastructure.MongoDb.Repositories;

/// <summary>
/// Repository for working with bike models in MongoDB
/// </summary>
public class MongoBikeModelRepository : IRepository<BikeModel, int>
{
    private readonly IMongoCollection<BikeModel> _collection;

    public MongoBikeModelRepository(MongoDbContext context)
    {
        _collection = context.BikeModels;

        var indexKeysDefinition = Builders<BikeModel>.IndexKeys.Ascending(m => m.Id);
        _collection.Indexes.CreateOne(new CreateIndexModel<BikeModel>(indexKeysDefinition));
    }

    public int Create(BikeModel entity)
    {
        var maxId = _collection.Find(_ => true)
            .SortByDescending(m => m.Id)
            .Limit(1)
            .FirstOrDefault()?.Id ?? 0;

        entity.Id = maxId + 1;
        _collection.InsertOne(entity);
        return entity.Id;
    }

    public List<BikeModel> ReadAll()
    {
        return _collection.Find(_ => true).ToList();
    }

    public BikeModel? Read(int id)
    {
        return _collection.Find(m => m.Id == id).FirstOrDefault();
    }

    public BikeModel? Update(int id, BikeModel entity)
    {
        entity.Id = id;
        var result = _collection.ReplaceOne(m => m.Id == id, entity);
        return result.ModifiedCount > 0 ? entity : null;
    }

    public bool Delete(int id)
    {
        var result = _collection.DeleteOne(m => m.Id == id);
        return result.DeletedCount > 0;
    }
}
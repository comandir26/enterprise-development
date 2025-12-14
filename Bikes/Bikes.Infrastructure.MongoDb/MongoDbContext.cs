using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Bikes.Domain.Models;
using Bikes.Infrastructure.MongoDb.Configuration;

namespace Bikes.Infrastructure.MongoDb;

/// <summary>
/// Context of connecting to MongoDB
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        _database = mongoClient.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Bike> Bikes => _database.GetCollection<Bike>("bikes");
    public IMongoCollection<BikeModel> BikeModels => _database.GetCollection<BikeModel>("bike_models");
    public IMongoCollection<Renter> Renters => _database.GetCollection<Renter>("renters");
    public IMongoCollection<Rent> Rents => _database.GetCollection<Rent>("rents");
}
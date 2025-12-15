using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Bikes.Domain.Models;
using Bikes.Infrastructure.MongoDb.Configuration;

namespace Bikes.Infrastructure.MongoDb;

/// <summary>
/// The context of connecting to MongoDB. 
/// </summary>
public class MongoDbContext
{
    /// <summary>
    /// Link to the MongoDB database
    /// </summary>
    private readonly IMongoDatabase _database;

    /// <summary>
    /// A constructor that accepts settings via the IOptions pattern.
    /// </summary>
    /// <param name="settings">MongoDB connection settings transmitted from the AppHost</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        if (settings?.Value == null)
            throw new ArgumentNullException(nameof(settings), "MongoDbSettings is not configured");

        var mongoDbSettings = settings.Value;

        if (string.IsNullOrEmpty(mongoDbSettings.ConnectionString))
            throw new ArgumentException("MongoDB connection string is not configured");

        if (string.IsNullOrEmpty(mongoDbSettings.DatabaseName))
            mongoDbSettings.DatabaseName = "BikesDB";

        var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
        _database = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
    }

    /// <summary>
    /// Bike collection. Corresponds to the "bikes" collection in MongoDB.
    /// </summary>
    public IMongoCollection<Bike> Bikes => _database.GetCollection<Bike>("bikes");

    /// <summary>
    /// A collection of bike models. Corresponds to the collection "bike_models" in MongoDB.
    /// </summary>
    public IMongoCollection<BikeModel> BikeModels => _database.GetCollection<BikeModel>("bike_models");

    /// <summary>
    /// Collection of renters. Corresponds to the "renters" collection in MongoDB
    /// </summary>
    public IMongoCollection<Renter> Renters => _database.GetCollection<Renter>("renters");

    /// <summary>
    /// A collection of rental records. Corresponds to the "rents" collection in MongoDB.
    /// </summary>
    public IMongoCollection<Rent> Rents => _database.GetCollection<Rent>("rents");
}
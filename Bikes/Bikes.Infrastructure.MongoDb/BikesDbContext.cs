using Bikes.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Bikes.Infrastructure.MongoDb;

/// <summary>
/// Database context for working with MongoDB
/// </summary>
public class BikesDbContext : DbContext
{
    public BikesDbContext(DbContextOptions<BikesDbContext> options) : base(options)
    {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }

    public DbSet<BikeModel> BikeModels => Set<BikeModel>();
    public DbSet<Bike> Bikes => Set<Bike>();
    public DbSet<Renter> Renters => Set<Renter>();
    public DbSet<Rent> Rents => Set<Rent>();

    /// <summary>
    /// Configuring the database model.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BikeModel>().ToCollection("bike_models");
        modelBuilder.Entity<Bike>().ToCollection("bikes");
        modelBuilder.Entity<Renter>().ToCollection("renters");
        modelBuilder.Entity<Rent>().ToCollection("rents");

        modelBuilder.Entity<Bike>()
            .Ignore(b => b.Model);

        modelBuilder.Entity<Rent>()
            .Ignore(r => r.Renter)
            .Ignore(r => r.Bike);
    }
}
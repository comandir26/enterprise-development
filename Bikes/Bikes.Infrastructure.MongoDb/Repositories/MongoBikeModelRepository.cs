using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Infrastructure.MongoDb.Repositories;

/// <summary>
/// A repository for working with bike models in MongoDB
/// </summary>
public class MongoBikeModelRepository : IRepository<BikeModel, int>
{
    private readonly BikesDbContext _context;

    public MongoBikeModelRepository(BikesDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="entity">Object</param>
    /// <returns>ID of the created object</returns>
    public int Create(BikeModel entity)
    {
        if (entity.Id == 0)
        {
            var lastId = _context.BikeModels
                .OrderByDescending(b => b.Id)
                .Select(b => b.Id)
                .FirstOrDefault();

            entity.Id = lastId + 1;
        }

        _context.BikeModels.Add(entity);
        _context.SaveChanges();
        return entity.Id;
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<BikeModel> ReadAll()
    {
        return _context.BikeModels.ToList();
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Object if exist</returns>
    public BikeModel? Read(int id)
    {
        return _context.BikeModels.Find(id);
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Object</param>
    /// <returns>Object if exist</returns>
    public BikeModel? Update(int id, BikeModel entity)
    {
        var existingModel = _context.BikeModels.Find(id);
        if (existingModel == null) return null;

        _context.Entry(existingModel).CurrentValues.SetValues(entity);

        existingModel.Id = id;

        _context.SaveChanges();
        return existingModel;
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool Delete(int id)
    {
        var model = _context.BikeModels.Find(id);
        if (model == null) return false;

        _context.BikeModels.Remove(model);
        _context.SaveChanges();
        return true;
    }
}
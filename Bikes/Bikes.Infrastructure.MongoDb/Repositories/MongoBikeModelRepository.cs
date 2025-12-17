using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Infrastructure.MongoDb.Repositories;

/// <summary>
/// A repository for working with bike models in MongoDB
/// </summary>
public class MongoBikeModelRepository(
    BikesDbContext context) : IRepository<BikeModel, int>
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="entity">Object</param>
    /// <returns>ID of the created object</returns>
    public int Create(BikeModel entity)
    {
        if (entity.Id == 0)
        {
            var lastId = context.BikeModels
                .OrderByDescending(b => b.Id)
                .Select(b => b.Id)
                .FirstOrDefault();

            entity.Id = lastId + 1;
        }

        context.BikeModels.Add(entity);
        context.SaveChanges();
        return entity.Id;
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<BikeModel> ReadAll()
    {
        return [.. context.BikeModels];
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Object if exist</returns>
    public BikeModel? Read(int id)
    {
        return context.BikeModels.Find(id);
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Object</param>
    /// <returns>Object if exist</returns>
    public BikeModel? Update(int id, BikeModel entity)
    {
        var existingModel = context.BikeModels.Find(id);
        if (existingModel == null) return null;

        context.Entry(existingModel).CurrentValues.SetValues(entity);

        existingModel.Id = id;

        context.SaveChanges();
        return existingModel;
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool Delete(int id)
    {
        var model = context.BikeModels.Find(id);
        if (model == null) return false;

        context.BikeModels.Remove(model);
        context.SaveChanges();
        return true;
    }
}
namespace Bikes.Domain.Repositories;

/// <summary>
/// Repository interface for CRUD operations with domain objects.
/// </summary>
/// <typeparam name="TEntity">Type of entity</typeparam>
/// <typeparam name="TKey">Type of identifier</typeparam>
public interface IRepository<TEntity, TKey>
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="entity">Object</param>
    /// <returns>ID of the created object</returns>
    public TKey Create(TEntity entity);

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<TEntity> ReadAll();

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Object if exist</returns>
    public TEntity? Read(TKey id);

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Object</param>
    /// <returns>Object if exist</returns>
    public TEntity? Update(TKey id, TEntity entity);

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool Delete(TKey id);
}

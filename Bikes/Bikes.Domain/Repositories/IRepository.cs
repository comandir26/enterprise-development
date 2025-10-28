namespace Bikes.Domain.Repositories;

public interface IRepository<TEntity, TKey>
{
    public TKey Create(TEntity entity);

    public List<TEntity> ReadAll();

    public TEntity? Read(TKey id);

    public TEntity? Update(TKey id, TEntity entity);

    public bool Delete(TKey id);
}

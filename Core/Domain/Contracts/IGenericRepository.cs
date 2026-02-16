using Domain.Entities.Shared;

namespace Domain.Contracts;

public interface IGenericRepository<TEntity,TKey> where TEntity : BasedEntity<TKey>
{
    //GetAll
    public Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);
    //GetById
    public Task<TEntity>? GetByIdAsync(TKey id);
    //Add
    public Task AddAsync(TEntity entity);
    //Update
    public void Update(TEntity entity);
    //Remove
    public void Remove(TEntity entity);

}
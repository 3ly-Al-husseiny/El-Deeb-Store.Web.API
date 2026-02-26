using Domain.Entities.Shared;

namespace Domain.Contracts;

public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
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

    #region Specifications

    Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity,TKey> specifications, bool asNoTracking = false);
    Task<TEntity?> GetByIdWithSpecAsync(ISpecifications<TEntity,TKey> specifications);
    Task<int> CountAsync(ISpecifications<TEntity,TKey> specifications);
    #endregion

}
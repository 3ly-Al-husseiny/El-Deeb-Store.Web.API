using Domain.Contracts;
using Domain.Entities.Shared;
using Presistence.Data;
using Presistence.Helpers;

namespace Presistence.Repositories;

public class GenericRepository<TEntity, TKey>(ECommerceDbContext _dbContext)
    : IGenericRepository<TEntity, TKey> where TEntity : BasedEntity<TKey>
{
    public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
    {
        return asNoTracking
            ? await _dbContext.Set<TEntity>().AsTracking().ToListAsync()
            : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<TEntity>? GetByIdAsync(TKey id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    #region Specification Design Pattern

    public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> specifications,
        bool asNoTracking = false)
        => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>().AsQueryable(), specifications)
            .ToListAsync();


    public async Task<TEntity?> GetByIdWithSpecAsync(ISpecifications<TEntity, TKey> specifications)
    => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>().AsQueryable(), specifications).FirstOrDefaultAsync();

    public async Task<int> CountAsync(ISpecifications<TEntity, TKey> specifications)
    {
        return await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>().AsQueryable(), specifications)
            .CountAsync();
    }

    #endregion
}
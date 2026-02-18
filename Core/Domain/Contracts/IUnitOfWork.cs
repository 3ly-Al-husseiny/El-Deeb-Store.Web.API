using Domain.Entities.Shared;

namespace Domain.Contracts;

public interface IUnitOfWork
{
    //Complete , SaveChanges
    public Task<int> SaveChangesAsync();
    // Method return obj from generic repo [Entity]
    public IGenericRepository<TEntity,TKey> GetGenericRepository<TEntity,TKey>() where TEntity : BasedEntity<TKey>;
}
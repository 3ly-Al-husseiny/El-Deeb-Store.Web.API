using System.Collections.Concurrent;
using Presistence.Data;

namespace Presistence.Repositories;

public class UnitOfWork(ECommerceDbContext _dbContext) : IUnitOfWork
{
    // private Dictionary<string, object> _repositories = new();
    private ConcurrentDictionary<string, object> _concurrentRepositories = new();


    public IGenericRepository<TEntity, TKey> GetGenericRepository<TEntity, TKey>() where TEntity : BasedEntity<TKey>
    {
        // will Create new obj every time we call this method , I Can Call it more than once for the same request. 
        // Not Efficient , I will use Dictionary to store the created obj and return it if exist.
        // return new GenericRepository<TEntity, TKey>(_dbContext);

        #region Normal Dictionary

        // var key = typeof(TEntity).Name; // Get the name of the entity type as the key
        // if (!_repositories.ContainsKey(key))
        // {
        //     var repository = new GenericRepository<TEntity, TKey>(_dbContext);
        //     _repositories[key] = repository; // Store the created repository in the dictionary
        //     return repository;
        // }
        //
        // return
        //     (IGenericRepository<TEntity, TKey>)_repositories[
        //         key]; // Cast the object back to the correct type and return it

        #endregion

        #region Concurrent Dictionary

        return (IGenericRepository<TEntity, TKey>)
            _concurrentRepositories.GetOrAdd(typeof(TEntity).Name, (_) => new ConcurrentDictionary<string, object>());

        #endregion
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
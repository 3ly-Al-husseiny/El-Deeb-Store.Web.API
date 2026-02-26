namespace Persistence.Helpers;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery,
        ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
    {
        var query = inputQuery;
        if (specifications.Criteria != null)
        {
            query = query.Where(specifications.Criteria);
        }

        if (specifications.OrderBy != null)
            query = query.OrderBy(specifications.OrderBy);

        if (specifications.OrderByDescending != null)
            query = query.OrderByDescending(specifications.OrderByDescending);

        if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
        {
            query = specifications.IncludeExpressions.Aggregate(query,
                (current, include) => current.Include(include));
        }

        if (specifications.isPaginated)
        {
            query = query.Skip(specifications.Skip).Take(specifications.Take);
        }

        return query;
    }
}
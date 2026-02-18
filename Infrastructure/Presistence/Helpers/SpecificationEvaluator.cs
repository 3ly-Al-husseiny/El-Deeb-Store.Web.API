namespace Presistence.Helpers;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery,
        ISpecifications<TEntity, TKey> specifications) where TEntity : BasedEntity<TKey>
    {
        var query = inputQuery;
        if (specifications.Criteria != null)
        {
            query = query.Where(specifications.Criteria);
        }

        if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
        {
            query = specifications.IncludeExpressions.Aggregate(query,
                (current, include) => current.Include(include));
        }
        return query;
    }
}
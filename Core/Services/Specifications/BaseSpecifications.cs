using System.Linq.Expressions;
using Domain.Contracts;
using Domain.Entities.Shared;

namespace Services.Specifications;

public abstract class BaseSpecifications<TEntity,TKey> : ISpecifications<TEntity,TKey> where TEntity : BasedEntity<TKey>
    
{
    protected BaseSpecifications(Expression<Func<TEntity, bool>>? criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<TEntity, bool>>? Criteria { get; private set; }
    public List<Expression<Func<TEntity, object>>>? IncludeExpressions { get; } = new();

    //AddIncludes(p => p.ProductBrand)  [Expression ==> Include]
    //AddIncludes(p => p.ProductType)  [Expression ==> Include]
    protected void AddIncludes(Expression<Func<TEntity, object>> includeExpression)
    {
        IncludeExpressions.Add(includeExpression);
    }
}
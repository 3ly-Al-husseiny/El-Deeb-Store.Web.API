using System.Linq.Expressions;
using Domain.Entities.Shared;

namespace Domain.Contracts;

public interface ISpecifications<TEntity,TKey> where TEntity : BasedEntity<TKey>
{
    //Signature for property [Expression ==> Where]
    public Expression<Func<TEntity,bool>>? Criteria { get; }
     //Signature for property [Expression ==> Include]
     public List<Expression<Func<TEntity, object>>>? IncludeExpressions { get; }
     //Signature for property [Expression ==> OrderBy]
     public Expression<Func<TEntity, object>>? OrderBy { get; }
     //Signature for property [Expression ==> OrderByDescending]
     public Expression<Func<TEntity, object>>? OrderByDescending { get; }
     
     //Pagination [Skip - Take] [ints]
     public bool isPaginated { get; }
     public int Skip { get; }
     public int Take { get; }
}
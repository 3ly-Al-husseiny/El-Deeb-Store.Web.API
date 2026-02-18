using System.Linq.Expressions;
using Domain.Entities.ProductModule;

namespace Services.Specifications;

public class ProductWithTypeAndBrandSpecifications : BaseSpecifications<Product,int>
{
    
    //Get All Products ==> Include Types , Brands [Include ==> AddInclude]
    public ProductWithTypeAndBrandSpecifications() : base(null)
    {
        AddIncludes(p => p.ProductType);
        AddIncludes(p => p.ProductBrand);
    }
    
    // Get Product By Id ==> Include Types , Brands [Include ==> AddInclude]
    public ProductWithTypeAndBrandSpecifications(Expression<Func<Product, bool>> criteria) : base(criteria)
    {
        AddIncludes(p => p.ProductType);
        AddIncludes(p => p.ProductBrand);
    }
    
    
}
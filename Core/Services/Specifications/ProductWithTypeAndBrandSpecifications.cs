using System.Linq.Expressions;
using Domain.Entities.ProductModule;
using Shared.Enums;

namespace Services.Specifications;

public class ProductWithTypeAndBrandSpecifications : BaseSpecifications<Product, int>
{
    //Get All Products ==> Include Types , Brands [Include ==> AddInclude]
    public ProductWithTypeAndBrandSpecifications(int? typeId, int? brandId, ProductSortingOptions sort)
        : base(p => (!typeId.HasValue || p.TypeId == typeId) && (!brandId.HasValue || p.BrandId == brandId))
    //where(p => p.BrandId == brandId && p => p.TypeId == typeId)  [Expression ==> Where]
    {
        AddIncludes(p => p.ProductType);
        AddIncludes(p => p.ProductBrand);

        //Switch Case For Sorting Options [Expression ==> OrderBy || OrderByDescending]
        switch (sort)
        {
            case ProductSortingOptions.PriceAsc:
                AddOrderBy(p => p.Price);
                break;
            case ProductSortingOptions.PriceDesc:
                AddOrderByDescending(p => p.Price);
                break;
            case ProductSortingOptions.NameDesc:
                AddOrderByDescending(p => p.Name);
                break;
            case ProductSortingOptions.NameAsc:
                AddOrderBy(p => p.Name);
                break;
        }
    }

    // Get Product By Id ==> Include Types , Brands [Include ==> AddInclude]
    public ProductWithTypeAndBrandSpecifications(Expression<Func<Product, bool>> criteria) : base(criteria)
    {
        AddIncludes(p => p.ProductType);
        AddIncludes(p => p.ProductBrand);
    }
}
using System.Linq.Expressions;
using Domain.Entities.ProductModule;
using Shared.EndPointsSpecificationsParameters;

namespace Services.Specifications;

public class ProductCountSpecifications : BaseSpecifications<Product, int>
{
    public ProductCountSpecifications(ProductSpecificationParameter parameter) : base(p =>
        (!parameter.TypeId.HasValue || p.TypeId == parameter.TypeId) &&
        (!parameter.BrandId.HasValue || p.BrandId == parameter.BrandId) &&
        (string.IsNullOrEmpty(parameter.Search) || p.Name.ToLower().Contains(parameter.Search.ToLower())))
    {
        // We don't need to include any related entities here because we are only counting the products, and we don't need any related data for that.
        // We just need to apply the filters based on the provided parameters (TypeId, BrandId, Search) to get the correct count of products that match those criteria.
    }
}
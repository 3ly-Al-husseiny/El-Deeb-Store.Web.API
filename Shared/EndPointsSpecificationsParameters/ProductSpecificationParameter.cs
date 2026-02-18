using Shared.Enums;

namespace Shared.EndPointsSpecificationsParameters;

public class ProductSpecificationParameter
{
    public int? TypeId { get; set; }
    public int? BrandId { get; set; }
    public ProductSortingOptions Sort { get; set; }
}
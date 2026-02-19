using System.Security.AccessControl;
using Shared.Enums;

namespace Shared.EndPointsSpecificationsParameters;

public class ProductSpecificationParameter
{
    private const int defaultPageSize = 5;
    private const int maxPageSize = 10;

    public int? TypeId { get; set; }
    public int? BrandId { get; set; }
    public string? Search { get; set; }
    public ProductSortingOptions Sort { get; set; }


    public int PageIndex { get; set; } = 1;
    private int _pageSize = defaultPageSize;

    public int PageSize
    {
        get => _pageSize;
        set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
    }
}
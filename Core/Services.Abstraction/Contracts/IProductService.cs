using Shared.DTOs.ProductModuleDTOs;
using Shared.EndPointsSpecificationsParameters;
using Shared.Enums;

namespace Services.Abstraction;

public interface IProductService
{
    //GetAllProducts --> Task<IEnumerable<ProductResultDto>>
    public Task<PaginatedResult<ProductResultDto>> GetAllAsync(ProductSpecificationParameter parameter);

    //GetAllBrands --> Task<IEnumerable<BrandResultDto>>
    public Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();

    //GetAllTypes
    public Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

    //GetProductById
    public Task<ProductResultDto>? GetByIdAsync(int id);
}
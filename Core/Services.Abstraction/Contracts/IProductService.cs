using Shared.DTOs.ProductModuleDTOs;

namespace Services.Abstraction;

public interface IProductService
{
    //GetAllProducts --> Task<IEnumerable<ProductResultDto>>
    public Task<IEnumerable<ProductResultDto>> GetAllAsync();

    //GetAllBrands --> Task<IEnumerable<BrandResultDto>>
    public Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();

    //GetAllTypes
    public Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

    //GetProductById
    public Task<ProductResultDto>? GetByIdAsync(int id);
}
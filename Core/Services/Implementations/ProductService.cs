using AutoMapper;
using Domain.Contracts;
using Domain.Entities.ProductModule;
using Services.Abstraction;
using Services.Specifications;
using Shared.DTOs.ProductModuleDTOs;
using Shared.Enums;

namespace Services.Implementations;

public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
{
    public async Task<IEnumerable<ProductResultDto>> GetAllAsync(int? typeId, int? brandId, ProductSortingOptions sort)
    {
        var productRepo = _unitOfWork.GetGenericRepository<Product, int>();
        // var products = await productRepo.GetAllAsync();
        var products = await productRepo.GetAllWithSpecAsync(new ProductWithTypeAndBrandSpecifications(typeId, brandId ,sort));
        return _mapper.Map<IEnumerable<ProductResultDto>>(products);
    }

    public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
    {
        var brandRepo = _unitOfWork.GetGenericRepository<ProductBrand,int>();
        var brands = await brandRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<BrandResultDto>>(brands);
    }

    public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
    {
        var typeRepo = _unitOfWork.GetGenericRepository<ProductType,int>();
        var types = await typeRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<TypeResultDto>>(types);
    }

    public async Task<ProductResultDto>? GetByIdAsync(int id)
    {
        var productRepo = _unitOfWork.GetGenericRepository<Product, int>();
        // var product = await productRepo.GetByIdAsync(id);
        var product = await productRepo.GetByIdWithSpecAsync(new ProductWithTypeAndBrandSpecifications(p => p.Id == id));
        if (product is null) return null;
        return _mapper.Map<ProductResultDto>(product);
    }
}
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Services.Abstraction;
using Services.Specifications;
using Shared.DTOs.ProductModuleDTOs;
using Shared.EndPointsSpecificationsParameters;
using Shared.Enums;

namespace Services.Implementations;

public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
{
    public async Task<PaginatedResult<ProductResultDto>> GetAllAsync(ProductSpecificationParameter parameter)
    {
        var productRepo = _unitOfWork.GetGenericRepository<Product, int>();
        var specifications = new ProductWithTypeAndBrandSpecifications(parameter);
        var products = await productRepo.GetAllWithSpecAsync(specifications);
        var productsResult = _mapper.Map<IEnumerable<ProductResultDto>>(products);
        var countSpecification = new ProductCountSpecifications(parameter);
        var totalCount = await productRepo.CountAsync(countSpecification);
        return new PaginatedResult<ProductResultDto>(parameter.PageIndex, parameter.PageSize, totalCount, productsResult);
    }

    public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
    {
        var brandRepo = _unitOfWork.GetGenericRepository<ProductBrand, int>();
        var brands = await brandRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<BrandResultDto>>(brands);
    }

    public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
    {
        var typeRepo = _unitOfWork.GetGenericRepository<ProductType, int>();
        var types = await typeRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<TypeResultDto>>(types);
    }

    public async Task<ProductResultDto>? GetByIdAsync(int id)
    {
        var productRepo = _unitOfWork.GetGenericRepository<Product, int>();
        // var product = await productRepo.GetByIdAsync(id);
        var product =
            await productRepo.GetByIdWithSpecAsync(new ProductWithTypeAndBrandSpecifications(p => p.Id == id));
        if (product is null) throw new ProductNotFoundException(id);
        return _mapper.Map<ProductResultDto>(product);
    }
}
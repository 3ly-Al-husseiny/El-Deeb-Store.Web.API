using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.DTOs.ProductModuleDTOs;
using Shared.Enums;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IServiceManager _serviceManager) : ControllerBase
{
    //EndPoint ==> Get AllProducst
    [HttpGet()] //BaseUrl/Producst [GET]
    public async Task<ActionResult<IEnumerable<ProductResultDto>>> GetAllProducstAsync(int? typeId , int? brandId , ProductSortingOptions sort)
    {
        var products = await _serviceManager.ProductService.GetAllAsync(typeId, brandId , sort);
        return Ok(products);
    }

    //EndPoint ==> Get AllBrands
    [HttpGet("Brands")] //BaseUrl/Producst/brands [GET]
    public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrandsAsync()
    {
        var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
        return Ok(brands);
    }

    //EndPoint ==> Get AllTypes
    [HttpGet("Types")] //BaseUrl/Producst/types [GET]
    public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypesAsync()
    {
        var types = await _serviceManager.ProductService.GetAllTypesAsync();
        return Ok(types);
    }
    
    //EndPoint ==> Get Product By Id
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResultDto>> GetProductByIdAsync(int id)
    {
        var product = await _serviceManager.ProductService.GetByIdAsync(id)!;
        return Ok(product);
    }
}
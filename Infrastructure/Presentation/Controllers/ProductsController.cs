using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.DTOs.ProductModuleDTOs;
using Shared.EndPointsSpecificationsParameters;
using Shared.Enums;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IServiceManager _serviceManager) : ControllerBase
{
    //EndPoint ==> Get AllProducst
    [HttpGet()] //BaseUrl/Producst [GET]
    public async Task<ActionResult<PaginatedResult<ProductResultDto>>> GetAllProducstAsync(
        [FromQuery] ProductSpecificationParameter parameter)
    {
        return Ok(await _serviceManager.ProductService.GetAllAsync(parameter));
    }

    //EndPoint ==> Get AllBrands
    [HttpGet("Brands")] //BaseUrl/Producst/brands [GET]
    public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrandsAsync()
    {
        return Ok(await _serviceManager.ProductService.GetAllBrandsAsync());
    }

    //EndPoint ==> Get AllTypes
    [HttpGet("Types")] //BaseUrl/Producst/types [GET]
    public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypesAsync()
    {
        return Ok(await _serviceManager.ProductService.GetAllTypesAsync());
    }

    //EndPoint ==> Get Product By Id
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResultDto>> GetProductByIdAsync(int id)
    {
        return Ok(await _serviceManager.ProductService.GetByIdAsync(id)!);
    }
}
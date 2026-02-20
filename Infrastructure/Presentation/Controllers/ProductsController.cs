using eCommerce.WebAPI.ErrorModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.DTOs.ProductModuleDTOs;
using Shared.EndPointsSpecificationsParameters;
using Shared.Enums;

namespace Presentation.Controllers;

public class ProductsController(IServiceManager _serviceManager) : ApiController
{
    /// <summary>
    /// Gets all products with pagination, filtering, and sorting options.
    /// </summary>
    /// <param name="parameter">Query parameters for filtering, sorting, and pagination.</param>
    /// <returns>A paginated list of products.</returns>
    [ProducesResponseType(type: typeof(PaginatedResult<ProductResultDto>), statusCode: StatusCodes.Status200OK)]
    [HttpGet()]
    public async Task<ActionResult<PaginatedResult<ProductResultDto>>> GetAllProducstAsync(
        [FromQuery] ProductSpecificationParameter parameter) =>
        Ok(await _serviceManager.ProductService.GetAllAsync(parameter));


    /// <summary>
    /// Gets all available product brands.
    /// </summary>
    /// <returns>A list of all product brands.</returns>
    [ProducesResponseType(type: typeof(IEnumerable<BrandResultDto>), statusCode: StatusCodes.Status200OK)]
    [HttpGet("Brands")]
    public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrandsAsync() =>
        Ok(await _serviceManager.ProductService.GetAllBrandsAsync());


    /// <summary>
    /// Gets all available product types.
    /// </summary>
    /// <returns>A list of all product types.</returns>
    [ProducesResponseType(type: typeof(IEnumerable<TypeResultDto>), statusCode: StatusCodes.Status200OK)]
    [HttpGet("Types")]
    public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypesAsync() =>
        Ok(await _serviceManager.ProductService.GetAllTypesAsync());


    /// <summary>
    /// Gets a specific product by its ID.
    /// </summary>
    /// <param name="id">The product ID.</param>
    /// <returns>The product details if found.</returns>
    /// 
    [ProducesResponseType(type: typeof(ProductResultDto), statusCode: StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResultDto>> GetProductByIdAsync(int id) =>
        Ok(await _serviceManager.ProductService.GetByIdAsync(id)!);
}
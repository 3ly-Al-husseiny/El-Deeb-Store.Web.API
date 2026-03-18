using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.DTOs.BasketModule;

namespace Presentation.Controllers;

public class BasketController(IServiceManager _serviceManager) : ApiController
{
    // Get BaseUrl/api/basket?id={id}
    [HttpGet]
    public async Task<ActionResult<BasketDto>> GetBasketAsync(string id) =>
        Ok(await _serviceManager.BasketService.GetBasketAsync(id));
    
    // Post BaseUrl/api/basket with body of BasketDto
    [HttpPost]
    public async Task<ActionResult<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basketDto) =>
        Ok(await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basketDto));
    
    // Delete BaseUrl/api/basket/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBasketAsync(string id)
    {
        await _serviceManager.BasketService.DeleteBasketAsync(id);
        return NoContent();
    }


}
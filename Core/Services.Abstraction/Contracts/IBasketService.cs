using Shared.DTOs.BasketModule;

namespace Services.Abstraction;

public interface IBasketService
{
    //Get 
    Task<BasketDto> GetBasketAsync(string id);
    //Delete
    Task<bool> DeleteBasketAsync(string id);
    //Update
    Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto);
}
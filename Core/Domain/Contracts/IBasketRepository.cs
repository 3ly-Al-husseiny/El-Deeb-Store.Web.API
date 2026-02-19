using Domain.Entities.BasketModule;

namespace Domain.Contracts;

public interface IBasketRepository
{
    // Get basket by id 
    Task<CustomerBasket?> GetBasketAsync(string id);
    // Create or update basket
    Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive);
    // Delete basket by id
    Task<bool> DeleteBasketAsync(string id); // Delete can be Async with redis
}
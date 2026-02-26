using System.Text.Json;
using Domain.Entities.BasketModule;
using StackExchange.Redis;

namespace Persistence.Repositories;

public class BasketRepository(IConnectionMultiplexer _connection) : IBasketRepository
{
    private readonly IDatabase _database = _connection.GetDatabase();


    public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive)
    {
        var jsonBasket = JsonSerializer.Serialize(basket);
        var result = await _database.StringSetAsync(basket.Id, jsonBasket, timeToLive ?? TimeSpan.FromDays(30));
        return (result) ? await GetBasketAsync(basket.Id) : null;
    }

    public async Task<CustomerBasket?> GetBasketAsync(string id)
    {
        var result = await _database.StringGetAsync(id);
        return result.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(result.ToString());
    }


    public async Task<bool> DeleteBasketAsync(string id)
    {
        return await _database.KeyDeleteAsync(id);
    }
}
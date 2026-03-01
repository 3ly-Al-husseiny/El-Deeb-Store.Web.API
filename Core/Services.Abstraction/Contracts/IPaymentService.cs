using Shared.DTOs.BasketModule;

namespace Services.Abstraction;

public interface IPaymentService
{
    Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId);
}
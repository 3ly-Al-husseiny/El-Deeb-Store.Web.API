using Shared.DTOs.OrderModule;

namespace Services.Abstraction;

public interface IOrderService
{
    // Get Order By Id ==> Take id ==> Return OrderResult
    Task<OrderResult> GetOrderByIdAsync(Guid id);
    // Get All Orders By Email ==> Take Email ==> Return IEnumerable<OrderResult>
    Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string userId);
    // Create Order ==> Take OrderCreate ==> Return OrderResult
    Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest , string userEmail);
    // Get Delivery Methods ==> Return IEnumerable<DeliveryMethodResult>
    Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync();
}
namespace Shared.DTOs.OrderModule;

public record OrderResult
{
    public Guid Id { get; init; }
    public string UserEmail { get; init; } = string.Empty;
    public AddressDto Address { get; init; } = new AddressDto();
    public ICollection<OrderItemDto> OrderItems { get; init; } = new List<OrderItemDto>();
    public string PaymentStatus { get; init; } = string.Empty;
    public string DeliveryMethod { get; init; } = string.Empty;
    public int? DeliveryMethodId { get; init; }
    public decimal Subtotal { get; init; }
    public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.UtcNow;
    public string PaymentIntentId { get; init; } = string.Empty;
    public decimal TotalPrice { get; init; } // Subtotal + DeliveryPrice
    
}
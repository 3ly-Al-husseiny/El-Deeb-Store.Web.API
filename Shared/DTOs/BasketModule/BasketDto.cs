namespace Shared.DTOs.BasketModule;

public record BasketDto
{
    // init --> to make the property immutable after initialization
    public string Id { get; init; }
    
    public ICollection<BasketItemDto> Items { get; init; } = [];
    public string? PaymentIntentId { get; init; }
    public string? ClientSecret { get; init; }
    public decimal? ShippingPrice { get; init; } //DeliveryMethod.Price
    public int? DeliveryMethodId { get; init; }
}
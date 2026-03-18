namespace Domain.Entities.BasketModule;

public class CustomerBasket
{
    public string Id { get; set; }
    public ICollection<BasketItem> Items { get; set; } = [];
    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }
    public decimal? ShippingPrice { get; set; } //DeliveryMethod.Price
    public int? DeliveryMethodId { get; set; }
}
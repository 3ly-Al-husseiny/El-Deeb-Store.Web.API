namespace Shared.DTOs.OrderModule;

public record OrderRequest()
{
    public string BasketId { get; init; } = string.Empty;
    public AddressDto ShippingAddress { get; init; } = new AddressDto();
    public int DeliveryMethodId { get; init; }
};
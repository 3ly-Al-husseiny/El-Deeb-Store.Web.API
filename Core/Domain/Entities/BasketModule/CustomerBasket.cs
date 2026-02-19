namespace Domain.Entities.BasketModule;

public class CustomerBasket
{
    public string Id { get; set; }
    public ICollection<BasketItem> BasketItems { get; set; } = [];
}
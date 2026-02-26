using Domain.Entities.Shared;

namespace Domain.Entities.OrderModule;

public class OrderItem : BaseEntity<Guid> 
{
    public OrderItem()
    {
        
    }

    public OrderItem(decimal price , int quantity)
    {
        Price = price;
        Quantity = quantity;
    }
    
    public ProductInOrderItem Product { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
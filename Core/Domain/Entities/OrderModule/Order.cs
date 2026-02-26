using System.Net.Mail;
using Domain.Entities.Shared;
using  ShippingAddress = Domain.Entities.OrderModule.Address;
namespace Domain.Entities.OrderModule;

public class Order : BaseEntity<Guid>
{
    public string UserEmail { get; set; } = string.Empty;
    public ShippingAddress ShippingAddress { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Peinding;
    
    public DeliveryMethod DeliveryMethod { get; set; }
    public int? DeliveryMethodId { get; set; }
    
    public decimal SubTotal { get; set; } //SubTotal = OrderItem * quantity * price
    
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    
    public string PaymentIntentId { get; set; } = string.Empty;
}
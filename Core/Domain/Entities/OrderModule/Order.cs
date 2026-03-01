using System.Net.Mail;
using Domain.Entities.Identity;
using Domain.Entities.Shared;
using ShippingAddress = Domain.Entities.OrderModule.Address;

namespace Domain.Entities.OrderModule;

public class Order : BaseEntity<Guid>
{
    public Order()
    {
    }

    public Order(decimal subTotal, DeliveryMethod deliveryMethod, ShippingAddress shippingAddress,
        ICollection<OrderItem> orderItems, string userEmail , string paymentIntentId)
    {
        Id = Guid.NewGuid();
        UserEmail = userEmail;
        ShippingAddress = shippingAddress;
        OrderItems = orderItems;
        DeliveryMethod = deliveryMethod;
        SubTotal = subTotal;
        PaymentIntentId = paymentIntentId;
    }

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
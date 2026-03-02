using System.Linq.Expressions;
using Domain.Entities.OrderModule;

namespace Services.Specifications;

public class OrderWithPaymentIntentIdSpecifications : BaseSpecifications<Order,Guid>
{
    public OrderWithPaymentIntentIdSpecifications(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
    {
    }
}
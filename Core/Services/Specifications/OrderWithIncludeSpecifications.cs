using System.Linq.Expressions;
using Domain.Entities.OrderModule;

namespace Services.Specifications;

public class OrderWithIncludeSpecifications : BaseSpecifications<Order,Guid>
{
    // Get Order By Id ==> cretria ==> id == o.Id ==> Includes (Deliverymethod, orderItems)
    public OrderWithIncludeSpecifications(Guid id) : base(o => o.Id == id)
    {
        AddIncludes(o => o.DeliveryMethod);
        AddIncludes(o => o.OrderItems);
        AddOrderBy(o => o.OrderDate);
    }
    
    // Get All OrdersByEmail ==> cretria ==> email == o.email ==> Includes (DeliveryMethod , order Items)
    public OrderWithIncludeSpecifications(string userEmail) : base(o => o.UserEmail == userEmail)
    {
        AddIncludes(o => o.DeliveryMethod);
        AddIncludes(o => o.OrderItems);
        AddOrderBy(o => o.OrderDate);
    }
}
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Services.Abstraction;
using Services.Specifications;
using Shared.DTOs.OrderModule;

namespace Services.Implementations;

public class OrderService(IMapper _mapper, IBasketRepository _basketRepo, IUnitOfWork _unitOfWork) : IOrderService
{
    public async Task<OrderRequest> GetOrderByIdAsync(Guid id)
    {
        var order = await _unitOfWork.GetGenericRepository<Order, Guid>()
            .GetByIdWithSpecAsync(new OrderWithIncludeSpecifications(id)) ?? throw new OrderNotFoundException(id);
        return _mapper.Map<OrderRequest>(order);
    }

    public async Task<IEnumerable<OrderRequest>> GetOrdersByEmailAsync(string userEmail)
    {
        var orders = await _unitOfWork.GetGenericRepository<Order, Guid>()
            .GetAllWithSpecAsync(new OrderWithIncludeSpecifications(userEmail));
        return  _mapper.Map<IEnumerable<OrderRequest>>(orders);
    }
    

    public async Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail)
    {
        //1] Map addressDto to address
        var address = _mapper.Map<Address>(orderRequest.ShippingAddress);
        //2] GetOrderItems ==> BasketId ==> Basket ==> BasketItems [Id]
        var basket = await _basketRepo.GetBasketAsync(orderRequest.BasketId) ??
                     throw new NotFoundException(orderRequest.BasketId);
        var orderItems = new List<OrderItem>();
        foreach (var item in basket.BasketItems)
        {
            var product = await _unitOfWork.GetGenericRepository<Product, int>().GetByIdAsync(item.Id) ??
                          throw new ProductNotFoundException(item.Id);
            orderItems.Add(CreateOrderItem(product, item));
        }

        //3] GetDeliveryMethod ==> DeliveryMethodId ==> DB
        var deliveryMethod = await _unitOfWork.GetGenericRepository<DeliveryMethod, int>()
                                 .GetByIdAsync(orderRequest.DeliveryMethodId)
                             ?? throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);
        //4] Calculate SubTotal ==> OrderItems ==> OrderItem.Q * OrderItem.Pirce
        var subTotal = orderItems.Sum(o => o.Price * o.Quantity);
        //5] Create Obj from order ==> params , Add DB , Save Changes
        var orderToCreate = new Order(subTotal, deliveryMethod, address, orderItems, userEmail , basket.PaymentIntentId);
        await _unitOfWork.GetGenericRepository<Order, Guid>().AddAsync(orderToCreate);
        await _unitOfWork.SaveChangesAsync();
        //6] Map <Order , OrderResult>
        return _mapper.Map<OrderResult>(orderToCreate);
    }

    public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
    {
        var deliveryMethods = await _unitOfWork.GetGenericRepository<DeliveryMethod, int>()
            .GetAllAsync();
        return _mapper.Map<IEnumerable<DeliveryMethodResult>>(deliveryMethods);
    }

    private OrderItem CreateOrderItem(Product product, BasketItem item)
    {
        var productInOrderItem = new ProductInOrderItem(product.Id, product.Name, product.PictureUrl);
        return new OrderItem(productInOrderItem, product.Price, item.Quantity);
    }
}
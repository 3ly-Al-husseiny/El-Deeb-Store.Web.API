using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.DTOs.OrderModule;

namespace Presentation.Controllers;

[Authorize]
public class OrdersController(IServiceManager _serviceManager) : ApiController
{
    // CreateOrder
    [HttpPost]
    public async Task<ActionResult<OrderResult>> CreateOrderAsync(OrderRequest orderRequest)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var order = await _serviceManager.OrderService.CreateOrderAsync(orderRequest, userEmail);
        return  Ok(order);
    }
    // GetOrderById
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResult>> GetOrderByIdAsync(Guid id)
    {
        var order = _serviceManager.OrderService.GetOrderByIdAsync(id);
        return  Ok(order);
    }
    
    // GetAllOrderByEmail
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResult>>> GetAllOrdersByEmailAsync()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var orders = _serviceManager.OrderService.GetOrdersByEmailAsync(userEmail);
        return  Ok(orders);
    }

    //Get Delivery Methods
    [HttpGet("DeliveryMethods")]
    public async Task<ActionResult<IEnumerable<DeliveryMethodResult>>> GetDeliveryMethodsAsync()
    {
        var deliveryMethods = await _serviceManager.OrderService.GetDeliveryMethodsAsync();
        return Ok(deliveryMethods);
    }
}
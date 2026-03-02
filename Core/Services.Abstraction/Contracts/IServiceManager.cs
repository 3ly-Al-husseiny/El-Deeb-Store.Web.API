namespace Services.Abstraction;

public interface IServiceManager
{
    public IProductService ProductService { get;} // Get only property for ProductService
    public IBasketService BasketService { get;}
    public IAuthenticationService AuthenticationService { get;}
    public IOrderService OrderService { get;}
    public IPaymentService PaymentService { get;}
    
}
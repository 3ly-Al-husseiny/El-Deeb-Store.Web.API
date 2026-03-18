using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction;
using Services.Specifications;
using Shared.DTOs.BasketModule;
using Stripe;
using Product = Domain.Entities.ProductModule.Product;
using Order = Domain.Entities.OrderModule.Order;


namespace Services.Implementations;

public class PaymentService(
    IConfiguration _configuration,
    IBasketRepository _basketRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper)
    : IPaymentService
{
    // public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
    // {
    //     //0] Install strip.net package [Done]
    //     //1] set up key [secret key]
    //     StripeConfiguration.ApiKey = _configuration.GetSection("StripeSetting")["SecretKey"];
    //     //2] get basket [by basketId]
    //     var basket = await _basketRepository.GetBasketAsync(basketId) ?? throw new BasketNotFoundException(basketId);
    //     //3] validate items price ==> [basket.item.price == product.price] ==> product from db
    //     foreach (var item in basket.BasketItems)
    //     {
    //         var product = await _unitOfWork.GetGenericRepository<Product, int>().GetByIdAsync(item.Id)!;
    //         item.Price = product.Price;
    //     }
    //
    //     //4] validate shipping price ==> get deliveryMethod [DeliveryMethodId] ==> ShippingPrice = DeliveryMethod.Price
    //     if (!basket.DeliveryMethodId.HasValue) throw new Exception("No delivery method selected");
    //     var deliveryMethod = await _unitOfWork.GetGenericRepository<DeliveryMethod, int>()
    //                              .GetByIdAsync(basket.DeliveryMethodId.Value)
    //                          ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
    //     basket.ShippingPrice = deliveryMethod.Price;
    //     //5] Total ==> [SubTotal + ShippingPrice] ==> Cent ==> * 100 ==> Long
    //     //              ==> 100 * [basket.items.q * basket.items.price + shippingPrice [DeliveryMethodPrice]
    //     var amout = (long)(basket.BasketItems.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;
    //     //6] Create Or Update PaymentIntentId 
    //     var stripService = new PaymentIntentService();
    //     if (string.IsNullOrEmpty(basket.PaymentIntentId))
    //     {
    //         // create
    //         var options = new PaymentIntentCreateOptions()
    //         {
    //             Amount = amout,
    //             Currency = "USD,",
    //             PaymentMethodTypes = ["card"]
    //         };
    //         var paymentIntent = await stripService.CreateAsync(options);
    //         basket.PaymentIntentId = paymentIntent.Id;
    //         basket.ClientSecret = paymentIntent.ClientSecret;
    //     }
    //     else
    //     {
    //         // update
    //         var options = new PaymentIntentUpdateOptions()
    //         {
    //             Amount = amout
    //         };
    //         await stripService.UpdateAsync(basket.PaymentIntentId, options);
    //     }
    //
    //     //7] Save Changes [update] Basket
    //     await _basketRepository.CreateOrUpdateBasketAsync(basket, TimeSpan.FromDays(7));
    //     //8] Map to basketDto ==> return
    //     return _mapper.Map<BasketDto>(basket);
    // }

    public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
    {
        StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];
        var basket = await GetBasketAsync(basketId);
        await ValidateBasketItemsPricesAsync(basket);
        var amout = CalculateTotalAmountAsync(basket);
        await CreationOrUpdatingPaymentIntentAsync(basket, amout);
        await _basketRepository.CreateOrUpdateBasketAsync(basket, TimeSpan.FromDays(7));
        return _mapper.Map<BasketDto>(basket);
    }

    public async Task UpdatePaymentStatusAsync(string json, string signatureHeader)
    {
        string endpointSecret = _configuration.GetSection("StripeSettings")["EndPointSecret"];
        var stripeEvent = EventUtility.ParseEvent(json,throwOnApiVersionMismatch:false);
        stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);
        // Handle the event
        // If on SDK version < 46, use class Events instead of EventTypes
        
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        
        if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
        {
            //Change order payment status ==> paymentRecieved
            await UpdatePaymentStatusRecievedAsync(paymentIntent.Id);

        }
        
        else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
        {
            await UpdatePaymentStatusFailedAsync(paymentIntent.Id);
        }
        
        else
        {
            // Unexpected event type
            Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
        }
    }

    private async Task UpdatePaymentStatusFailedAsync(string paymentIntentId)
    {

        var orderRepo = _unitOfWork.GetGenericRepository<Order, Guid>();
        var order = await orderRepo.GetByIdWithSpecAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));
        if (order is not null)
        {
            order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
            orderRepo.Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
       
    }

    private async Task UpdatePaymentStatusRecievedAsync(string paymentIntentId)
    {
        var orderRepo = _unitOfWork.GetGenericRepository<Order, Guid>();
        var order = await orderRepo.GetByIdWithSpecAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));
        if (order is not null)
        {
            order.PaymentStatus = OrderPaymentStatus.PaymentReceived;
            orderRepo.Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    private async Task CreationOrUpdatingPaymentIntentAsync(CustomerBasket basket, long amout)
    {
        var stripService = new PaymentIntentService();
        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            // create
            var options = new PaymentIntentCreateOptions()
            {
                Amount = amout,
                Currency = "USD",
                PaymentMethodTypes = ["card"]
            };
            var paymentIntent = await stripService.CreateAsync(options);
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;
        }
        else
        {
            // update
            var options = new PaymentIntentUpdateOptions()
            {
                Amount = amout
            };
            await stripService.UpdateAsync(basket.PaymentIntentId, options);
        }
    }

    private long CalculateTotalAmountAsync(CustomerBasket basket)
    {
        return (long)(basket.Items.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;
    }

    private async Task ValidateBasketItemsPricesAsync(CustomerBasket basket)
    {
        // validate the BasketItems Prices
        foreach (var item in basket.Items)
        {
            var product = await _unitOfWork.GetGenericRepository<Product, int>().GetByIdAsync(item.Id)
                          ?? throw new ProductNotFoundException(item.Id);
            item.Price = product.Price;
        }

        // validate the Delivery Prices
        if (!basket.DeliveryMethodId.HasValue) throw new Exception("No delivery method selected");
        var deliveryMethod = await _unitOfWork.GetGenericRepository<DeliveryMethod, int>()
                                 .GetByIdAsync(basket.DeliveryMethodId.Value)
                             ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
        basket.ShippingPrice = deliveryMethod.Price;
    }

    private async Task<CustomerBasket> GetBasketAsync(string basketId)
    {
        return await _basketRepository.GetBasketAsync(basketId)
               ?? throw new BasketNotFoundException(basketId);
    }
}
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.OrderModule;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction;
using Shared.DTOs.BasketModule;
using Stripe;
using Product = Domain.Entities.ProductModule.Product;

namespace Services.Implementations;

public class PaymentService(
    IConfiguration _configuration,
    IBasketRepository _basketRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper)
    : IPaymentService
{
    public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
    {
        //0] Install strip.net package [Done]
        //1] set up key [secret key]
        StripeConfiguration.ApiKey = _configuration.GetSection("StripeSetting")["SecretKey"];
        //2] get basket [by basketId]
        var basket = await _basketRepository.GetBasketAsync(basketId) ?? throw new BasketNotFoundException(basketId);
        //3] validate items price ==> [basket.item.price == product.price] ==> product from db
        foreach (var item in basket.BasketItems)
        {
            var product = await _unitOfWork.GetGenericRepository<Product, int>().GetByIdAsync(item.Id)!;
            item.Price = product.Price;
        }

        //4] validate shipping price ==> get deliveryMethod [DeliveryMethodId] ==> ShippingPrice = DeliveryMethod.Price
        if (!basket.DeliveryMethodId.HasValue) throw new Exception("No delivery method selected");
        var deliveryMethod = await _unitOfWork.GetGenericRepository<DeliveryMethod, int>()
                                 .GetByIdAsync(basket.DeliveryMethodId.Value)
                             ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
        basket.ShippingPrice = deliveryMethod.Price;
        //5] Total ==> [SubTotal + ShippingPrice] ==> Cent ==> * 100 ==> Long
        //              ==> 100 * [basket.items.q * basket.items.price + shippingPrice [DeliveryMethodPrice]
        var amout = (long)(basket.BasketItems.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;
        //6] Create Or Update PaymentIntentId 
        var stripService = new PaymentIntentService();
        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            // create
            var options = new PaymentIntentCreateOptions()
            {
                Amount = amout,
                Currency = "USD,",
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

        //7] Save Changes [update] Basket
        await _basketRepository.CreateOrUpdateBasketAsync(basket,TimeSpan.FromDays(7));
        //8] Map to basketDto ==> return
        return _mapper.Map<BasketDto>(basket);
    }
}
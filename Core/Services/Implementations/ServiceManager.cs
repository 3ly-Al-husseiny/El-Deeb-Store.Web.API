using AutoMapper;
using Domain.Contracts;
using Services.Abstraction;

namespace Services.Implementations;

public class ServiceManager(IUnitOfWork _uniOfWork, IMapper _mapper, IBasketRepository _basketRepo) : IServiceManager
{
    private readonly Lazy<IProductService> _productService =
        new Lazy<IProductService>(() =>
            new ProductService(_uniOfWork, _mapper)); // Lazy initialization for ProductService


    private readonly Lazy<IBasketService> _basketService =
        new Lazy<IBasketService>(() => new BasketService(_basketRepo, _mapper));

    public IProductService ProductService => _productService.Value;
    public IBasketService BasketService => _basketService.Value;
}
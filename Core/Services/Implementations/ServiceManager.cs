using AutoMapper;
using Domain.Contracts;
using Services.Abstraction;

namespace Services.Implementations;

public class ServiceManager(IUnitOfWork _uniOfWork, IMapper _mapper) : IServiceManager
{
    private readonly Lazy<IProductService> _productService =
        new Lazy<IProductService>(() =>
            new ProductService(_uniOfWork, _mapper)); // Lazy initialization for ProductService


    public IProductService ProductService => _productService.Value;
}
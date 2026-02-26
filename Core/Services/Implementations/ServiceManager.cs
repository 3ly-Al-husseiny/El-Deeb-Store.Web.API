using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Abstraction;
using Shared.Common;

namespace Services.Implementations;

public class ServiceManager(
    IUnitOfWork _uniOfWork,
    IMapper _mapper,
    IBasketRepository _basketRepo,
    UserManager<User> _userManager,
    IOptions<JwtOptions> _jwtOptions) : IServiceManager
{
    private readonly Lazy<IProductService> _productService =
        new Lazy<IProductService>(() =>
            new ProductService(_uniOfWork, _mapper)); // Lazy initialization for ProductService


    private readonly Lazy<IBasketService> _basketService =
        new Lazy<IBasketService>(() => new BasketService(_basketRepo, _mapper));

    private readonly Lazy<IAuthenticationService> _authenticationService =
        new Lazy<IAuthenticationService>(() => new AuthenticationService(_userManager, _mapper , _jwtOptions));

    public IProductService ProductService => _productService.Value;
    public IBasketService BasketService => _basketService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}
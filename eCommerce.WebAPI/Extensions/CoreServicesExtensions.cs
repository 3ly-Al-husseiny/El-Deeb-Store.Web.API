using Services;
using Services.Abstraction;
using Services.Implementations;

namespace eCommerce.WebAPI.Extensions;

public static class CoreServicesExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(ServicesAssemblyReference).Assembly);
        services.AddScoped<IServiceManager, ServiceManager>();
        return services;
    }
}
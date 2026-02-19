using Services;
using Services.Abstraction;
using Services.Implementations;

namespace eCommerce.WebAPI.Extensions;

public static class CoreServicesExtensions
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(ServicesAssemblyReference).Assembly);
        services.AddScoped<IServiceManager, ServiceManager>();
    }
}
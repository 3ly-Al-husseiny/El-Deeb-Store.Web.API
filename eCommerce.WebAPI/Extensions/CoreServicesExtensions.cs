using Services;
using Services.Abstraction;
using Services.Implementations;
using Shared.Common;

namespace eCommerce.WebAPI.Extensions;

public static class CoreServicesExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(cfg => { }, typeof(ServicesAssemblyReference).Assembly);
        services.AddScoped<IServiceManager, ServiceManager>();
        
        // Mapping JwtOptions from appsettings.json to JwtOptions class IOptions<JwtOptions>
        
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions")); //IOptions<JwtOptions>
        return services;
    }
}
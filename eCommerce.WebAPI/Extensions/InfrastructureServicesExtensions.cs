using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;
using Presistence.Repositories;
using Services.Abstraction;
using StackExchange.Redis;

namespace eCommerce.WebAPI.Extensions;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
// Register the DbContext with the dependency injection container and configure its options
        services.AddDbContext<ECommerceDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IDataSeeding, DataSeeding>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IConnectionMultiplexer>((_) =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!));
        services.AddScoped<IBasketRepository, BasketRepository>();


        return services;
    }
}
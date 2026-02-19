using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;
using Presistence.Repositories;
using Services.Abstraction;

namespace eCommerce.WebAPI.Extensions;

public static class InfrastructureServicesExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration)
    {
// Register the DbContext with the dependency injection container and configure its options
        services.AddDbContext<ECommerceDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IDataSeeding, DataSeeding>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
using eCommerce.WebAPI.Factories;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.WebAPI.Extensions;

public static class WebApiServicesExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
        });

        return services;
    }
}
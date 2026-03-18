using eCommerce.WebAPI.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;

namespace eCommerce.WebAPI.Extensions;

public static class WebApiServicesExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services , IConfiguration _configurations)
    {
        // Add services to the container.
        services.AddControllers();
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyHeader().AllowAnyMethod().WithOrigins(_configurations.GetSection("URLS")["FrontUrl"]);
            });
        });
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



    public static IServiceCollection ConfigureMySwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "El-Deeb E-Commerce Web API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer"),
                    new List<string>()
                }
            });
        });
        return services;
    }
}
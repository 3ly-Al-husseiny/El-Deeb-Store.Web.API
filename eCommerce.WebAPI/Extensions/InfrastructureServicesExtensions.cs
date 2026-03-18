using System.Text;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositories;
using Services.Abstraction;
using Shared.Common;
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

        // Identity DbContext registration
        // Register the DbContext with the dependency injection container and configure its options
        services.AddDbContext<IdentityECommerceDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
        });


        services.AddScoped<IDataSeeding, DataSeeder>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IConnectionMultiplexer>((_) =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!));
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireDigit = true;
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<IdentityECommerceDbContext>() /*.AddDefaultTokenProviders()*/;
        services.ValidateJwt(configuration);


        return services;
    }


    public static IServiceCollection ValidateJwt(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind the "JwtOptions" section of the configuration to a JwtOptions object
        var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
        services.AddAuthentication(options =>
        {
            // jwtBearerDefaults --> check who is the user and validate the token
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme; // Set the default authentication scheme to JWT Bearer

            // jwtBearerDefaults --> if the user is not authenticated and try to access a protected resource,
            // it will challenge the user to authenticate using JWT Bearer
            // will redirect to the login page or return a 401 Unauthorized response, depending on the client application
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });
        services.AddAuthorization();
        return services;
    }
}
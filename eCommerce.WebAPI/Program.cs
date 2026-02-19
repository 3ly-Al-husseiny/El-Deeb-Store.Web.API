using Domain.Contracts;
using eCommerce.WebAPI.Extensions;
using eCommerce.WebAPI.Factories;
using eCommerce.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presistence;
using Presistence.Data;
using Presistence.Repositories;
using Services;
using Services.Abstraction;
using Services.Implementations;

namespace eCommerce.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
        });


        // Register the DbContext with the dependency injection container and configure its options
        builder.Services.AddDbContext<ECommerceDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddScoped<IDataSeeding, DataSeeding>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        builder.Services.AddCoreServices();
        
        var app = builder.Build();

        // Seed Data with the first request to the API

        #region Data Seeding before the first request to the API

        using var scope = app.Services.CreateScope();
        var objOfDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
        objOfDataSeeding.SeedAsync();

        #endregion

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {

            app.UseMiddleware<GlobalExceptionHandlingMiddlewares>();
            
            app.MapOpenApi(); //Middleware to serve the registered OpenAPI/Swagger documents.

            app.UseSwagger(); //Middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwaggerUI(); //Middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        }

                
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.MapControllers();
        app.Run();
    }
}
using eCommerce.WebAPI.Middlewares;
using Services.Abstraction;

namespace eCommerce.WebAPI.Extensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
    {
        // Seed Data with the first request to the API

        #region Data Seeding before the first request to the API

        using var scope = app.Services.CreateScope();
        var objOfDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
        await objOfDataSeeding.SeedAsync();
        await objOfDataSeeding.SeedIdentityAsync();

        #endregion

        return app;
    }

    public static WebApplication UseExceptionHandlingMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        return app;
    }

    public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
    {
        app.MapOpenApi(); //Middleware to serve the registered OpenAPI/Swagger documents.
        app.UseSwagger(); //Middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwaggerUI(); //Middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        return app;
    }
}
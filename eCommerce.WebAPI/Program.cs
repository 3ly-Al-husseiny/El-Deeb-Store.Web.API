using eCommerce.WebAPI.Extensions;
namespace eCommerce.WebAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        #region Dependency Injection Container

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddWebApiServices(builder.Configuration);
        builder.Services.ConfigureMySwaggerGen();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddCoreServices(builder.Configuration);

        #endregion

        #region Middlewares - Pipeline

        var app = builder.Build();

        await app.SeedDatabaseAsync();
        app.UseExceptionHandlingMiddlewares();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerMiddlewares();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors("CorsPolicy");
        app.UseAuthentication(); 
        app.UseAuthorization();
        app.MapControllers();
        app.Run();

        #endregion
    }
}
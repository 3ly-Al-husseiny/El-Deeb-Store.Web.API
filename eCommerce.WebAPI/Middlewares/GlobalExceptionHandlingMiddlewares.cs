using System.Text.Json;
using Domain.Exceptions;
using eCommerce.WebAPI.ErrorModels;

namespace eCommerce.WebAPI.Middlewares;

public class GlobalExceptionHandlingMiddlewares
{
    private readonly ILogger<GlobalExceptionHandlingMiddlewares> _logger;
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddlewares(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddlewares> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Somthing went wrong ==> : {ex.Message}");
            await HandleExceptionAsync(context, ex);
        }
    }

    // Helper Method
    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        //1] Change StatusCode
        // context.Response.StatusCode = 200; // OK , As in real projects we will send the error in body with status code 200
        // context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.StatusCode = ex switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            (_) => StatusCodes.Status500InternalServerError
        };

        //2] Change Content Type
        context.Response.ContentType = "application/json";

        //3] Write Response in Body
        var response = new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            ErrorMessage =
                ex.Message, // ex.Message will be sent to client in production environment, but in development environment we can send ex.ToString() to get more details about the error
        }.ToString();
        await context.Response.WriteAsync(response);
    }
}
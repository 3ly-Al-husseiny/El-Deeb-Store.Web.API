using System.Text.Json;
using Domain.Exceptions;
using eCommerce.WebAPI.ErrorModels;

namespace eCommerce.WebAPI.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); //404 product not found throw ex , 500 internal server error throw ex
            
            // Handle 404 Not Found for endpoints that are not found 
            if(context.Response.StatusCode == StatusCodes.Status404NotFound)
                await HandleNotFoundAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Somthing went wrong ==> : {ex.Message}");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleNotFoundAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var response = new ErrorDetails()
        {
            StatusCode = StatusCodes.Status404NotFound,
            ErrorMessage = $"The endpoint with url {context.Request.Path} is not found."
        }.ToString();
        await context.Response.WriteAsync(response);
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
            UnAuthorizedException => StatusCodes.Status401Unauthorized, 
                ValidationException => StatusCodes.Status400BadRequest,
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
using eCommerce.WebAPI.ErrorModels;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.WebAPI.Factories;

public class ApiResponseFactory
{
    public static IActionResult CustomValidationErrorResponse(ActionContext context)
    {
        // context ==> Used widely with MVC
        // context ==> errors , key [Field]
        // context ==> ModelState ==> <string,ModelStateEntry>
        // string ==> Name of the field that has the error
        // ModelStateEntry ==> Errors property that contains the list of errors for that field (Complex Object)
        // We need to get the Error Messages from the ModelState and return them in a custom format

        // IEnumerable<ValidationError> errors = context.ModelState
        var errors = context.ModelState
            .Where(error => error.Value?.Errors.Any() == true).Select(error => new ValidationError()
            {
                Field = error.Key,
                Errors = error.Value?.Errors.Select(error => error.ErrorMessage) ?? []
            });
        
        var response = new ValidationErrorResponse()
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ErrorMessage = "One or More validation errors happened.",
            Errors = errors
        };

        return new BadRequestObjectResult(response);


    }
}
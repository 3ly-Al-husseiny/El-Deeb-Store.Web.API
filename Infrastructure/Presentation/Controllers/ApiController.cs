using eCommerce.WebAPI.ErrorModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
[ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
[ProducesResponseType(type: typeof(ValidationErrorResponse), statusCode: StatusCodes.Status400BadRequest)]
public class ApiController : ControllerBase
{
}
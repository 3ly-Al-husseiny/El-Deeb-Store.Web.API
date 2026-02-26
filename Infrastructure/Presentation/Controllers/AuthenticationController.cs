using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Abstraction;
using Shared.Common;
using Shared.DTOs.IdentityModule;

namespace Presentation.Controllers;

public class AuthenticationController(IServiceManager _serviceManager ) : ApiController
{
    //Post ==> Login
    [HttpPost("Login")]
    public async Task<ActionResult<UserResultDto>> LoginAsync(LoginDto loginDto)
    => Ok(await _serviceManager.AuthenticationService.LoginAsync(loginDto));
    
    //Post ==> Register
    [HttpPost("Register")]
    public async Task<ActionResult<UserResultDto>> RegisterAsync(RegisterDto registerDto)
        => Ok(await _serviceManager.AuthenticationService.RegisterAsync(registerDto));
}
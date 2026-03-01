using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Abstraction;
using Shared.Common;
using Shared.DTOs.IdentityModule;
using Shared.DTOs.OrderModule;

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

    [HttpGet("EmailExist")]
    public async Task<ActionResult<bool>> CheckEmailExistAsync(string email)
    => Ok(await _serviceManager.AuthenticationService.CheckEmailExistAsync(email)); 
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserResultDto>> GetCurrentUserAsync()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        return Ok(await _serviceManager.AuthenticationService.GetCurrentUserAsync(userEmail));
    }

    [Authorize]
    [HttpGet("Address")]
    public async Task<ActionResult<AddressDto>> GetUserAddressAsync()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        return Ok(await _serviceManager.AuthenticationService.GetUserAddressAsync(email));
    }

    [Authorize]
    [HttpGet("Address")]
    public async Task<ActionResult<AddressDto>> UpdateUserAddressAsync(AddressDto addressDto)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var address = await _serviceManager.AuthenticationService.UpdateUserAddressAsync(email, addressDto);
        return  Ok(address);
    }
}
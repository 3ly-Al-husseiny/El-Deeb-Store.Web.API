using AutoMapper;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction;
using Shared.DTOs.IdentityModule;

namespace Services.Implementations;

public class AuthenticationService(UserManager<User> _userManager, IMapper _mapper) : IAuthenticationService
{
    public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
    {
        // Check if the user exists
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user is null)
        {
            throw new UnAuthorizedException();
        }

        // Check if the password is correct
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordValid)
        {
            throw new UnAuthorizedException();
        }

        return new UserResultDto(user.DisplayName, "FakeToken", user.Email);
    }

    public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
    {
        
    }
}
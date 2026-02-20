using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction;
using Shared.DTOs.IdentityModule;
using ValidationException = Domain.Exceptions.ValidationException;

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
        var user = new User()
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.UserName,
            PhoneNumber = registerDto.PhoneNumber
        };
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        // Validate
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            throw new ValidationException(errors);
        }

        return new UserResultDto(user.DisplayName, "FakeToken", user.Email);
    }
}
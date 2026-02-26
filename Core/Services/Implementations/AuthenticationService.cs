using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction;
using Shared.Common;
using Shared.DTOs.IdentityModule;
using ValidationException = Domain.Exceptions.ValidationException;

namespace Services.Implementations;

public class AuthenticationService(UserManager<User> _userManager, IMapper _mapper , IOptions<JwtOptions> _options) : IAuthenticationService
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

        return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
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

        return new UserResultDto(user.DisplayName,await CreateTokenAsync(user), user.Email);
    }
    
    
    //Token ==> Encrypted string ==> function return string
    //Helper method
    private async Task<string> CreateTokenAsync(User user)
    {
        
        var jwtOptions = _options.Value;
        
        //Claims
        //Names , Email , Roles [m-m]
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.DisplayName),
            new Claim(ClaimTypes.Email, user.Email!)
        };
        var roles = await _userManager.GetRolesAsync(user);
        
        // add the roles to the claims
        
        // claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        // or
        
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        // get secret key from JWT Secret website
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
        
        // Algorithm to encrypt the token - signin credentials
        var signInCredetionals = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer:jwtOptions.Issuer,
            audience:jwtOptions.Audience,
            claims: claims,
            // expires: DateTime.UtcNow.AddDays(30), // Depend on the requirement and the business
                expires: DateTime.UtcNow.AddDays(jwtOptions.ExpirationInDays), // Depend on the requirement and the business
            signingCredentials: signInCredetionals
        );
        
        // WriteToken --> JWTSecurityToken ==> string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
using Shared.DTOs.IdentityModule;

namespace Services.Abstraction;

public interface IAuthenticationService
{
    // Login(Email,Password) ==> UserResultDto [DisplayName, Token, Email]
    Task<UserResultDto> LoginAsync(LoginDto loginDto);
    // Register(Email,Password,PhoneNumber,UserName,DisplayName) ==> UserResultDto [DisplayName, Token, Email]
    Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
}
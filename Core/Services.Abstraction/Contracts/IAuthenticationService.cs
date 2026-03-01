using Shared.DTOs.IdentityModule;
using Shared.DTOs.OrderModule;

namespace Services.Abstraction;

public interface IAuthenticationService
{
    // Login(Email,Password) ==> UserResultDto [DisplayName, Token, Email]
    Task<UserResultDto> LoginAsync(LoginDto loginDto);

    // Register(Email,Password,PhoneNumber,UserName,DisplayName) ==> UserResultDto [DisplayName, Token, Email]
    Task<UserResultDto> RegisterAsync(RegisterDto registerDto);

    //Get current user
    Task<UserResultDto> GetCurrentUserAsync(string userEmail);

    //check if email exist 
    Task<bool> CheckEmailExistAsync(string userEmail);

    //get address
    Task<AddressDto> GetUserAddressAsync(string userEmail);

    //update address
    Task<AddressDto> UpdateUserAddressAsync(string userEmail, AddressDto addressDto);
}
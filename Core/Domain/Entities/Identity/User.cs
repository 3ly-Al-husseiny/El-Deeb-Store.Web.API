using System.Net.Sockets;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class User : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
    public Address Address { get; set; }
}
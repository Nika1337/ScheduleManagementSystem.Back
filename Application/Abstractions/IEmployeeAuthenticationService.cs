

using System.Security.Claims;

namespace Application.Abstractions;

public interface IEmployeeAuthenticationService
{
    Task<string> PasswordSignInAsync(string id, string password);
    Task ChangePasswordAsync(ClaimsPrincipal claimsPrincipal, string currentPassword, string newPassword);
}

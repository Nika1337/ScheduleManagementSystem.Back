

using System.Security.Claims;

namespace Application.Abstractions;

public interface IEmployeeAuthenticationService
{
    Task<string> PasswordSignInAsync(string id, string password);
    Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
}

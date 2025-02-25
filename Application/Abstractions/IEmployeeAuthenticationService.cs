


namespace Application.Abstractions;

public interface IEmployeeAuthenticationService
{
    Task<string> PasswordSignInAsync(string email, string password);
    Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
}

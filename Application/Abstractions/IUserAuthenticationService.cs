



using Application.DataTransferObjects.Users;

namespace Application.Abstractions;

public interface IUserAuthenticationService
{
    Task<UserAuthenticationResponse> AuthenticateAsync(string email, string password);
    Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task ChangeEmailAsync(Guid userId, string newEmail);
}

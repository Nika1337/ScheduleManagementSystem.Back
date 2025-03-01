


using Application.DataTransferObjects.Employees;

namespace Application.Abstractions;

public interface IEmployeeAuthenticationService
{
    Task<EmployeeAuthenticationResponse> AuthenticateAsync(string email, string password);
    Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
}

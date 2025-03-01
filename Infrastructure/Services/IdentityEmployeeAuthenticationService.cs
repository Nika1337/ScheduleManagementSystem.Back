using Application.Abstractions;
using Application.DataTransferObjects.Employees;
using Domain.Exceptions;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Validators;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

internal class IdentityEmployeeAuthenticationService : IEmployeeAuthenticationService
{
    private readonly UserManager<IdentityEmployee> _userManager;

    public IdentityEmployeeAuthenticationService(
        UserManager<IdentityEmployee> userManager)
    {
        _userManager = userManager;
    }

    public async Task<EmployeeAuthenticationResponse> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new NotFoundException($"User with email '{email}' not found.");

        if (!await _userManager.CheckPasswordAsync(user, password))
            throw new PasswordIncorrectException();

        var userRoleName = (await _userManager.GetRolesAsync(user)).First();

        var response = new EmployeeAuthenticationResponse
        {
            EmployeeId = user.Id,
            RoleName = userRoleName,
        };

        return response;
    }

    public async Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var existingEmployee = await _userManager.FindByIdAsync(userId.ToString())
                               ?? throw new NotFoundException($"User with id '{userId}' not found.");

        if (!await _userManager.CheckPasswordAsync(existingEmployee, currentPassword))
            throw new PasswordIncorrectException();

        var validator = new EmployeePasswordValidator();
        var validationResult = await validator.ValidateAsync(_userManager, existingEmployee, newPassword);

        if (!validationResult.Succeeded)
        {
            throw new PasswordStructureValidationException(validationResult.Errors.FirstOrDefault()?.Description ?? "");
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(existingEmployee, currentPassword, newPassword);

        if (!changePasswordResult.Succeeded)
        {
            throw new ApplicationException($"Unable to change password for employee with ID '{existingEmployee.Id}'");
        }
    }

}

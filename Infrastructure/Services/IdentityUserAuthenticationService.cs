using Application.Abstractions;
using Application.DataTransferObjects.Users;
using Domain.Exceptions;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Validators;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

internal class IdentityUserAuthenticationService : IUserAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityUserAuthenticationService(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserAuthenticationResponse> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new NotFoundException($"User with email '{email}' not found.");

        if (!await _userManager.CheckPasswordAsync(user, password))
            throw new PasswordIncorrectException();

        var userRoleName = (await _userManager.GetRolesAsync(user)).First();

        var response = new UserAuthenticationResponse
        {
            UserId = user.Id,
            RoleName = userRoleName,
            Email = email
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

        await _userManager.ChangePasswordAsync(existingEmployee, currentPassword, newPassword);
    }

    public async Task ChangeEmailAsync(Guid userId, string newEmail)
    {
        var existingEmployee = await _userManager.FindByIdAsync(userId.ToString())
                               ?? throw new NotFoundException($"User with id '{userId}' not found.");

        await _userManager.SetEmailAsync(existingEmployee, newEmail);
    }

}

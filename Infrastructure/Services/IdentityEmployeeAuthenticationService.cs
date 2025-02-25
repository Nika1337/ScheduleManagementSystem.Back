using Application.Abstractions;
using Domain.Exceptions;
using Infrastructure.Identity;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

internal class IdentityEmployeeAuthenticationService : IEmployeeAuthenticationService
{
    private readonly UserManager<IdentityEmployee> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public IdentityEmployeeAuthenticationService(
        UserManager<IdentityEmployee> userManager,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
    }

    public async Task<string> PasswordSignInAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new NotFoundException($"User with email '{email}' not found.");

        if (!await _userManager.CheckPasswordAsync(user, password))
            throw new PasswordIncorrectException();

        return await GenerateJwtTokenAsync(user);
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

    private async Task<string> GenerateJwtTokenAsync(IdentityEmployee user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds);

        return _tokenHandler.WriteToken(token);
    }
}

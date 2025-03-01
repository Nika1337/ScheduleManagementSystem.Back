using Application.Abstractions;
using Application.DataTransferObjects.Employees;
using Azure.Core;
using Domain.Exceptions;
using Infrastructure.Identity;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

internal class IdentityEmployeeService : IEmployeeService
{
    private static readonly string _temporaryPassword = "AppleOrange.1234";
    private readonly IdentityContext _dbContext;
    private readonly UserManager<IdentityEmployee> _userManager;

    public IdentityEmployeeService(UserManager<IdentityEmployee> userManager, IdentityContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task CreateEmployeeAsync(EmployeeCreateRequest request)
    {
        var identityEmployee = new IdentityEmployee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
            StartDateUtc = DateTime.UtcNow
        };

        var creationResult = await _userManager.CreateAsync(identityEmployee, _temporaryPassword);
        if (!creationResult.Succeeded)
        {
            var errors = string.Join(", ", creationResult.Errors.Select(e => e.Description));
            throw new ApplicationException($"Failed to register employee '{identityEmployee.FirstName} {identityEmployee.LastName}': {errors}");
        }

        var roleAdditionResult = await _userManager.AddToRoleAsync(identityEmployee, request.RoleName);
        if (!roleAdditionResult.Succeeded)
        {
            throw new ApplicationException($"Failed to assign role '{request.RoleName}' to employee '{identityEmployee.UserName}'.");
        }
    }

    public async Task<EmployeeProfileResponse> GetEmployeeProfileAsync(Guid id)
    {
        var identityEmployee = await GetIdentityEmployeeAsync(id);

        var response = new EmployeeProfileResponse 
        {
            FirstName = identityEmployee.FirstName,
            LastName = identityEmployee.LastName,
            Email = identityEmployee.Email!
        };

        return response;
    }

    public async Task<IEnumerable<EmployeeResponse>> GetEmployeesAsync()
    {
        var employees = await (from user in _dbContext.Users
                               join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                               join role in _dbContext.Roles on userRole.RoleId equals role.Id
                               select new EmployeeResponse
                               {
                                   Id = user.Id,
                                   FirstName = user.FirstName,
                                   LastName = user.LastName,
                                   Email = user.Email ?? "",
                                   StartDateUtc = user.StartDateUtc,
                                   RoleName = role.Name ?? ""
                               })
                       .ToListAsync();

        return employees;
    }

    public async Task UpdateEmployeeAsync(EmployeeUpdateByAdminRequest request)
    {
        var identityEmployee = await GetIdentityEmployeeAsync(request.Id);

        var currentRole = (await _userManager.GetRolesAsync(identityEmployee)).FirstOrDefault();
        var newRoleName = request.RoleName;

        // If role is the same, do nothing
        if (currentRole == newRoleName)
            return;

        // Remove existing role if different
        if (!string.IsNullOrEmpty(currentRole))
        {
            var roleRemovalResult = await _userManager.RemoveFromRoleAsync(identityEmployee, currentRole);
            if (!roleRemovalResult.Succeeded)
            {
                throw new ApplicationException($"Failed to remove role '{currentRole}' for employee with Id {identityEmployee.Id}");
            }
        }

        // Add the new role
        if (!string.IsNullOrEmpty(newRoleName))
        {
            var roleAdditionResult = await _userManager.AddToRoleAsync(identityEmployee, newRoleName);
            if (!roleAdditionResult.Succeeded)
            {
                throw new ApplicationException($"Failed to add role '{newRoleName}' for employee with Id {identityEmployee.Id}");
            }
        }

    }

    public async Task UpdateEmployeeAsync(EmployeeProfileUpdateRequest request)
    {
        var identityEmployee = await GetIdentityEmployeeAsync(request.Id);

        identityEmployee.FirstName = request.FirstName;
        identityEmployee.LastName = request.LastName;
        identityEmployee.Email = request.Email;

        var updateResult = await _userManager.UpdateAsync(identityEmployee);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            throw new ApplicationException($"Failed to update profile for employee with Id {identityEmployee.Id}: {errors}");
        }
    }

    private async Task<IdentityEmployee> GetIdentityEmployeeAsync(Guid id)
    {
        var identityEmployee = await _userManager.FindByIdAsync(id.ToString())
           ?? throw new NotFoundException($"Employee with Id '{id}' not found.");

        return identityEmployee;
    }
}



using Application.Abstractions;
using Application.DataTransferObjects.Employees;
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
            StartDateUtc = DateTime.UtcNow
        };

        var creationResult = await _userManager.CreateAsync(identityEmployee, _temporaryPassword);

        if (!creationResult.Succeeded)
        {
            throw new ApplicationException($"Unable to register employee with name '{identityEmployee.FirstName + " " + identityEmployee.LastName}'");
        }

        var roleAdditionResult = await _userManager.AddToRoleAsync(identityEmployee, request.RoleName);

        if (!roleAdditionResult.Succeeded)
        {
            throw new ApplicationException($"Unable to Add roles to employee with username '{identityEmployee.UserName}'");
        }
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
                       .GroupBy(e => e.Id)
                       .Select(g => g.First()) 
                       .ToListAsync();

        return employees;

    }

    public async Task UpdateEmployeeAsync(EmployeeUpdateByAdminRequest request)
    {
        var identityEmployee = await _userManager.FindByIdAsync(request.Id.ToString()) ?? throw new NotFoundException($"Employee with Id '{request.Id}' not found.");

        var currentRole = (await _userManager.GetRolesAsync(identityEmployee)).FirstOrDefault();

        var newRoleName = request.RoleName;

        // If the new role is the same as the current role, do nothing
        if (currentRole == newRoleName)
        {
            return;
        }

        // Remove existing role if it's different
        if (!string.IsNullOrEmpty(currentRole))
        {
            var roleRemovalResult = await _userManager.RemoveFromRoleAsync(identityEmployee, currentRole);
            if (!roleRemovalResult.Succeeded)
            {
                throw new ApplicationException($"Unable to remove role '{currentRole}' for employee with Id {identityEmployee.Id}");
            }
        }

        // Add the new role
        if (!string.IsNullOrEmpty(newRoleName))
        {
            var roleAdditionResult = await _userManager.AddToRoleAsync(identityEmployee, newRoleName);
            if (!roleAdditionResult.Succeeded)
            {
                throw new ApplicationException($"Unable to add role '{newRoleName}' for employee with Id {identityEmployee.Id}");
            }
        }
    }

    public async Task UpdateEmployeeAsync(EmployeeProfileUpdateRequest request)
    {
        var identityEmployee = await _userManager.FindByIdAsync(request.Id.ToString()) ?? throw new NotFoundException($"Employee with Id '{request.Id}' not found.");


        identityEmployee.FirstName = request.FirstName;
        identityEmployee.LastName = request.LastName;
        identityEmployee.Email = request.Email;
    }
}


using Domain.Abstractions;
using Domain.Exceptions;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

internal class WorkerService : IWorkerService
{
    private readonly UserManager<IdentityEmployee> _userManager;

    public WorkerService(UserManager<IdentityEmployee> userManager)
    {
        _userManager = userManager;
    }

    public async Task<(string FirstName, string LastName)> GetWorkerFullNameAsync(Guid workerId)
    {
        var employee = await _userManager.FindByIdAsync(workerId.ToString()) ?? throw new NotFoundException($"Employee with id '{workerId}' not found.");

        return (employee.FirstName, employee.LastName);
    }

    public async Task<Dictionary<Guid, (string FirstName, string LastName)>> GetWorkerFullNamesAsync(List<Guid> workerIds)
    {
        if (workerIds == null || workerIds.Count == 0)
        {
            return [];
        }

        var employees = await _userManager.Users
            .Where(e => workerIds.Contains(e.Id))
            .Select(e => new { e.Id, e.FirstName, e.LastName })
            .ToListAsync();

        if (employees.Count !=  workerIds.Count)
        {
            throw new NotFoundException("Some of employees from given ids not found");
        }

        var result = employees.ToDictionary(
            e => e.Id,
            e => (e.FirstName, e.LastName)
        );


        return result;
    }
}
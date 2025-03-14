﻿using Application.Abstractions;
using Application.DataTransferObjects.Workers;
using Domain.Abstractions;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Data.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

internal class IdentityWorkerService : IWorkerService
{
    private static readonly string _temporaryPassword = "AppleOrange.1234";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<Worker> _repository;

    public IdentityWorkerService(UserManager<ApplicationUser> userManager, IRepository<Worker> repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    public async Task CreateWorkerAsync(WorkerCreateRequest request)
    {
        var employee = new Worker
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            StartDate = DateTime.Now
        };

        await _repository.AddAsync(employee);


        var identityEmployee = new ApplicationUser
        {
            Id = employee.Id,
            Email = request.Email,
            UserName = request.Email
        };

        await _userManager.CreateAsync(identityEmployee, _temporaryPassword);

        await _userManager.AddToRoleAsync(identityEmployee, "Worker");
    }

    public async Task<WorkerProfileResponse> GetWorkerProfileAsync(Guid id)
    {
        var worker = await _repository.GetByIdAsync(id) ?? throw new NotFoundException($"Worker with Id '{id}' not found.");

        var response = new WorkerProfileResponse 
        {
            FirstName = worker.FirstName,
            LastName = worker.LastName,
        };

        return response;
    }

    public async Task<IEnumerable<WorkerResponse>> GetWorkersAsync()
    {
        var workersTask = _repository.ListAsync();
        var workerEmailsTask = _userManager.Users
            .Select(ie => new { ie.Id, ie.Email })
            .ToDictionaryAsync(obj => obj.Id, obj => obj.Email);

        await Task.WhenAll(workersTask, workerEmailsTask);

        var workers = await workersTask;
        var workerEmails = await workerEmailsTask;

        var response = workers.Select(worker => new WorkerResponse
        {
            Id = worker.Id,
            FirstName = worker.FirstName,
            LastName = worker.LastName,
            Email = workerEmails[worker.Id] ?? throw new NotFoundException($"User with id '{worker.Id}' not found"),
            StartDate = worker.StartDate
        });

        return response;
    }

    public async Task UpdateWorkerAsync(WorkerProfileUpdateRequest request)
    {
        var worker = await _repository.GetByIdAsync(request.Id) ?? throw new NotFoundException($"Worker with Id '{request.Id}' not found.");

        worker.FirstName = request.FirstName;
        worker.LastName = request.LastName;

        await _repository.UpdateAsync(worker);
    }
}

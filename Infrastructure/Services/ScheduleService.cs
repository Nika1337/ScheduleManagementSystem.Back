﻿
using Application.Abstractions;
using Application.DataTransferObjects.Schedules;
using Domain.Abstractions;
using Domain.Exceptions;
using Domain.Models;
using Domain.Specification.Schedules;

namespace Infrastructure.Services;

internal class ScheduleService : IScheduleService
{
    private readonly IRepository<Schedule> _repository;
    private readonly IRepository<Job> _jobRepository;
    private readonly IRepository<Worker> _workerRepository;

    public ScheduleService(IRepository<Schedule> repository, IRepository<Job> jobRepository, IRepository<Worker> workerRepository)
    {
        _repository = repository;
        _jobRepository = jobRepository;
        _workerRepository = workerRepository;
    }

    public async Task<IEnumerable<ScheduleDetailedResponse>> GetSchedulesAsync(DateOnly startDate, DateOnly endDate)
    {
        var specification = new ScheduleDetailedSpecification(startDate, endDate);

        var schedules = await _repository.ListAsync(specification);

        var response = schedules.Select(sch => new ScheduleDetailedResponse
        {
            Id = sch.Id,
            JobName = sch.JobName,
            WorkerFirstName = sch.WorkerFirstName,
            WorkerLastName = sch.WorkerLastName,
            Date = sch.Date,
            PartOfDay = sch.PartOfDay,
        });

        return response;
    }

    public async Task<IEnumerable<ScheduleDetailedResponse>> GetSchedulesForWorkerAsync(Guid workerId, DateOnly startDate, DateOnly endDate)
    {
        var specification = new ScheduleDetailedSpecification(workerId, startDate, endDate);

        var entities = await _repository.ListAsync(specification);
        

        var response = entities.Select(sch => new ScheduleDetailedResponse
        {
            Id = sch.Id,
            JobName = sch.JobName,
            WorkerFirstName = sch.WorkerFirstName,
            WorkerLastName = sch.WorkerLastName,
            Date = sch.Date,
            PartOfDay = sch.PartOfDay,
        });

        return response;
    }

    public async Task CreateScheduleAsync(ScheduleCreateRequest request)
    {
        var job = await _jobRepository.GetByIdAsync(request.JobId) ?? throw new NotFoundException($"Job with id '{request.JobId}' not found.");

        var worker = await _workerRepository.GetByIdAsync(request.WorkerId) ?? throw new NotFoundException($"Worker with id '{request.WorkerId}' not found.");

        var schedule = new Schedule
        {
            ScheduleOfWorker = worker,
            ScheduledAtDate = request.Date,
            ScheduledAtPartOfDay = request.PartOfDay,
            JobToPerform = job
        };

        await _repository.AddAsync(schedule);
    }

    public async Task ChangeScheduleAsync(ScheduleChangeRequest request)
    {
        var specification = new ScheduleWithPendingChangeByIdSpecification(request.Id);

        var schedule = await _repository.SingleOrDefaultAsync(specification) ?? throw new NotFoundException($"Schedule with id '{request.Id}' not found");

        schedule.PendingScheduleChange = null;

        schedule.ScheduledAtDate = request.Date;
        schedule.ScheduledAtPartOfDay = request.PartOfDay;

        await _repository.UpdateAsync(schedule);
    }

    public async Task RequestScheduleChangeAsync(ScheduleChangeByWorkerRequest request)
    {
        var specification = new ScheduleWithPendingChangeByIdSpecification(request.Id, request.WorkerId);

        var schedule = await _repository.SingleOrDefaultAsync(specification) ?? throw new NotFoundException($"Schedule with id '{request.Id}' not found");

        schedule.PendingScheduleChange = new PendingScheduleChange
        {
            ScheduleToChange = schedule,
            NewDate = request.Date,
            NewPartOfDay = request.PartOfDay,
            RequestDateTime = DateTime.Now,
        };

        await _repository.UpdateAsync(schedule);
    }
}

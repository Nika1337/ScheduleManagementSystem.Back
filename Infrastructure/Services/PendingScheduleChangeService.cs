using Application.Abstractions;
using Application.DataTransferObjects.PendingScheduleChanges;
using Application.DataTransferObjects.Schedules;
using Domain.Abstractions;
using Domain.Exceptions;
using Domain.Models;
using Domain.Specification.PendingScheduleChanges;

namespace Infrastructure.Services;

internal class PendingScheduleChangeService : IPendingScheduleChangeService
{
    private readonly IRepository<PendingScheduleChange> _repository;
    private readonly IScheduleNotificationService _scheduleNotificationService;

    public PendingScheduleChangeService(IRepository<PendingScheduleChange> repository, IScheduleNotificationService scheduleNotificationService)
    {
        _repository = repository;
        _scheduleNotificationService = scheduleNotificationService;
    }

    public async Task<IEnumerable<PendingScheduleChangeResponse>> GetPendingScheduleChangesAsync()
    {
        var specification = new PendingScheduleChangeDetailedSpecification();

        var entities = await _repository.ListAsync(specification);

        var response = entities.Select(scr => new PendingScheduleChangeResponse
        {
            Id = scr.Id,
            WorkerFirstName = scr.WorkerFirstName,
            WorkerLastName = scr.WorkerLastName,
            JobName = scr.JobName,
            PreviousDate = scr.PreviousDate,
            PreviousPartOfDay = scr.PreviousPartOfDay,
            NewDate = scr.NewDate,
            NewPartOfDay = scr.NewPartOfDay,
            RequestDateTime = scr.RequestDateTime
        });

        return response;
    }

    public async Task<IEnumerable<PendingScheduleChangeResponse>> GetPendingScheduleChangesAsync(Guid workerId)
    {
        var specification = new PendingScheduleChangeDetailedSpecification(workerId);

        var entities = await _repository.ListAsync(specification);

        var response = entities.Select(scr => new PendingScheduleChangeResponse
        {
            Id = scr.Id,
            WorkerFirstName = scr.WorkerFirstName,
            WorkerLastName = scr.WorkerLastName,
            JobName = scr.JobName,
            PreviousDate = scr.PreviousDate,
            PreviousPartOfDay = scr.PreviousPartOfDay,
            NewDate = scr.NewDate,
            NewPartOfDay = scr.NewPartOfDay,
            RequestDateTime = scr.RequestDateTime
        });

        return response;
    }

    public async Task AcceptPendingScheduleChange(Guid id)
    {
        var PendingScheduleChange = await GetPendingScheduleChange(id);

        var scheduleToChange = PendingScheduleChange.ScheduleToChange;

        var oldDate = scheduleToChange.ScheduledAtDate;
        var oldPartOfDay = scheduleToChange.ScheduledAtPartOfDay;

        scheduleToChange.ScheduledAtDate = PendingScheduleChange.NewDate;
        scheduleToChange.ScheduledAtPartOfDay = PendingScheduleChange.NewPartOfDay;

        await ResetPendingScheduleChange(scheduleToChange, PendingScheduleChange);

        var scheduleChangeEvent = new ScheduleChangeEvent
        {
            WorkerId = scheduleToChange.ScheduleOfWorker.Id,
            Message = $"Pending Schedule Change from {oldDate} {oldPartOfDay} to {PendingScheduleChange.NewDate} {PendingScheduleChange.NewPartOfDay} Accepted and changed for job {scheduleToChange.JobToPerform.Name}"
        };

        await _scheduleNotificationService.PublishChangeAsync(scheduleChangeEvent, default);
    }

    public async Task RejectPendingScheduleChange(Guid id)
    {
        var PendingScheduleChange = await GetPendingScheduleChange(id);

        var scheduleToChange = PendingScheduleChange.ScheduleToChange;

        var requestedDate = scheduleToChange.ScheduledAtDate;
        var requestedPartOfDay = scheduleToChange.ScheduledAtPartOfDay;

        await ResetPendingScheduleChange(scheduleToChange, PendingScheduleChange);

        var scheduleChangeEvent = new ScheduleChangeEvent
        {
            WorkerId = scheduleToChange.ScheduleOfWorker.Id,
            Message = $"Pending Schedule Change from {scheduleToChange.ScheduledAtDate} {scheduleToChange.ScheduledAtPartOfDay} to {requestedDate} {requestedPartOfDay} Rejected for job {scheduleToChange.JobToPerform.Name}"
        };

        await _scheduleNotificationService.PublishChangeAsync(scheduleChangeEvent, default);
    }

    public async Task WithdrawPendingScheduleChange(Guid id, Guid workerId)
    {
        var specification = new PendingScheduleChangeWithScheduleToChangeByIdSpecification(id, workerId);

        var PendingScheduleChange = await _repository.SingleOrDefaultAsync(specification) ?? throw new NotFoundException($"Schedule change request with id '{id}' and worker id '{workerId}' not found.");

        var scheduleToChange = PendingScheduleChange.ScheduleToChange;

        await ResetPendingScheduleChange(scheduleToChange, PendingScheduleChange);
    }


    private async Task<PendingScheduleChange> GetPendingScheduleChange(Guid id)
    {
        var specification = new PendingScheduleChangeWithScheduleToChangeByIdSpecification(id);

        var PendingScheduleChange = await _repository.SingleOrDefaultAsync(specification) ?? throw new NotFoundException($"Schedule change request with id '{id}' not found.");

        return PendingScheduleChange;
    }

    private async Task ResetPendingScheduleChange(Schedule schedule, PendingScheduleChange PendingScheduleChange)
    {
        schedule.PendingScheduleChange = null;

        await _repository.DeleteAsync(PendingScheduleChange);
    }
}

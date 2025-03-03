using Application.Abstractions;
using Application.DataTransferObjects.PendingScheduleChanges;
using Domain.Abstractions;
using Domain.Exceptions;
using Domain.Models;
using Domain.Specification.PendingScheduleChanges;

namespace Infrastructure.Services;

internal class PendingScheduleChangeService : IPendingScheduleChangeService
{
    private readonly IRepository<PendingScheduleChange> _repository;
    private readonly IWorkerService _workerService;

    public PendingScheduleChangeService(IRepository<PendingScheduleChange> repository, IWorkerService workerService)
    {
        _repository = repository;
        _workerService = workerService;
    }

    public async Task<IEnumerable<PendingScheduleChangeResponse>> GetPendingScheduleChangesAsync()
    {
        var specification = new PendingScheduleChangeDetailedSpecification();

        var entities = await _repository.ListAsync(specification);

        var workerFullNames = await _workerService.GetWorkerFullNamesAsync(entities.Select(e => e.WorkerId).ToList());

        var response = entities.Select(scr => new PendingScheduleChangeResponse
        {
            Id = scr.Id,
            WorkerFirstName = workerFullNames[scr.WorkerId].FirstName,
            WorkerLastName = workerFullNames[scr.WorkerId].LastName,
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

        var (FirstName, LastName) = await _workerService.GetWorkerFullNameAsync(workerId);

        var response = entities.Select(scr => new PendingScheduleChangeResponse
        {
            Id = scr.Id,
            WorkerFirstName = FirstName,
            WorkerLastName = LastName,
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

        scheduleToChange.ScheduledAtDate = PendingScheduleChange.NewDateUtc;
        scheduleToChange.ScheduledAtPartOfDay = PendingScheduleChange.NewPartOfDay;

        await ResetPendingScheduleChange(scheduleToChange, PendingScheduleChange);
    }

    public async Task RejectPendingScheduleChange(Guid id)
    {
        var PendingScheduleChange = await GetPendingScheduleChange(id);

        var scheduleToChange = PendingScheduleChange.ScheduleToChange;

        await ResetPendingScheduleChange(scheduleToChange, PendingScheduleChange);
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

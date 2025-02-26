using Application.Abstractions;
using Application.DataTransferObjects.PendingScheduleChanges;
using Domain.Abstractions;
using Domain.Exceptions;
using Domain.Models;
using Domain.Specification.PendingScheduleChanges;

namespace Infrastructure.Services;

internal class PendingScheduleChangeService : IPendingScheduleChangeService
{
    public readonly IRepository<PendingScheduleChange> _repository;

    public PendingScheduleChangeService(IRepository<PendingScheduleChange> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PendingScheduleChangeResponse>> GetPendingScheduleChangesAsync()
    {
        var specification = new PendingScheduleChangeDetailedSpecification();

        var entities = await _repository.ListAsync(specification);

        var response = entities.Select(scr => new PendingScheduleChangeResponse
        {
            Id = scr.Id,
            WorkerFirstName = "",
            WorkerLastName = "",
            JobName = scr.JobName,
            PreviousDate = scr.PreviousDate,
            PreviousPartOfDay = scr.PreviousPartOfDay,
            NewDate = scr.NewDate,
            NewPartOfDay = scr.NewPartOfDay,
            RequestDate = scr.RequestDateTime
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

    public async Task WithdrawPendingScheduleChange(Guid id)
    {
        var PendingScheduleChange = await GetPendingScheduleChange(id);

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

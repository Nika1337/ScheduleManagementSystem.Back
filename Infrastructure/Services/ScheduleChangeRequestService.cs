using Application.Abstractions;
using Application.DataTransferObjects.ScheduleChangeRequests;
using Domain.Abstractions;
using Domain.Exceptions;
using Domain.Models;
using Domain.Specification.ScheduleChangeRequests;

namespace Infrastructure.Services;

internal class ScheduleChangeRequestService : IScheduleChangeRequestService
{
    public readonly IRepository<ScheduleChangeRequest> _repository;

    public ScheduleChangeRequestService(IRepository<ScheduleChangeRequest> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ScheduleChangeRequestResponse>> GetScheduleChangeRequestsAsync()
    {
        var specification = new ScheduleChangeDetailedSpecification();

        var entities = await _repository.ListAsync(specification);

        var response = entities.Select(scr => new ScheduleChangeRequestResponse
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

    public async Task AcceptScheduleChangeRequest(Guid id)
    {
        var scheduleChangeRequest = await GetScheduleChangeRequest(id);

        var scheduleToChange = scheduleChangeRequest.ScheduleToChange;

        scheduleToChange.ScheduledAtDate = scheduleChangeRequest.NewDateUtc;
        scheduleToChange.ScheduledAtPartOfDay = scheduleChangeRequest.NewPartOfDay;

        await ResetScheduleChangeRequest(scheduleToChange, scheduleChangeRequest);
    }

    public async Task RejectScheduleChangeRequest(Guid id)
    {
        var scheduleChangeRequest = await GetScheduleChangeRequest(id);

        var scheduleToChange = scheduleChangeRequest.ScheduleToChange;

        await ResetScheduleChangeRequest(scheduleToChange, scheduleChangeRequest);
    }

    public async Task WithdrawScheduleChangeRequest(Guid id)
    {
        var scheduleChangeRequest = await GetScheduleChangeRequest(id);

        var scheduleToChange = scheduleChangeRequest.ScheduleToChange;

        await ResetScheduleChangeRequest(scheduleToChange, scheduleChangeRequest);
    }


    private async Task<ScheduleChangeRequest> GetScheduleChangeRequest(Guid id)
    {
        var specification = new ScheduleChangeRequestWithScheduleToChangeByIdSpecification(id);

        var scheduleChangeRequest = await _repository.SingleOrDefaultAsync(specification) ?? throw new NotFoundException($"Schedule change request with id '{id}' not found.");

        return scheduleChangeRequest;
    }

    private async Task ResetScheduleChangeRequest(Schedule schedule, ScheduleChangeRequest scheduleChangeRequest)
    {
        schedule.ScheduleChangeRequest = null;

        await _repository.DeleteAsync(scheduleChangeRequest);
    }
}

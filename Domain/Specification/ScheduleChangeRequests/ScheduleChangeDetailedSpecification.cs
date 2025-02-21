using Ardalis.Specification;
using Domain.Models;
using Domain.Specification.ScheduleChangeRequests.Results;

namespace Domain.Specification.ScheduleChangeRequests;

public class ScheduleChangeDetailedSpecification : BaseModelByIdSpecification<ScheduleChangeRequest, ScheduleChangeRequestDetailedResult>
{
    public ScheduleChangeDetailedSpecification(Guid id) : base(id)
    {
        Query.Select(scr => new ScheduleChangeRequestDetailedResult
        {
            Id = scr.Id,
            WorkerId = scr.scheduleToChange.ScheduleOfWorkerId,
            JobName = scr.scheduleToChange.JobToPerform.Name,
            PreviousDate = scr.scheduleToChange.ScheduledAtDate,
            PreviousPartOfDay = scr.scheduleToChange.ScheduledAtPartOfDay,
            NewDate = scr.NewDateUtc,
            NewPartOfDay = scr.NewPartOfDay,
            RequestDateTime = DateTime.MinValue 
        });
    }
}

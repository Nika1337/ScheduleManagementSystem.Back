using Ardalis.Specification;
using Domain.Models;
using Domain.Specification.ScheduleChangeRequests.Results;

namespace Domain.Specification.ScheduleChangeRequests;

public class ScheduleChangeDetailedSpecification : Specification<ScheduleChangeRequest, ScheduleChangeRequestDetailedResult>
{
    public ScheduleChangeDetailedSpecification()
    {
        Query.Select(scr => new ScheduleChangeRequestDetailedResult
        {
            Id = scr.Id,
            WorkerId = scr.ScheduleToChange.ScheduleOfWorkerId,
            JobName = scr.ScheduleToChange.JobToPerform.Name,
            PreviousDate = scr.ScheduleToChange.ScheduledAtDate,
            PreviousPartOfDay = scr.ScheduleToChange.ScheduledAtPartOfDay,
            NewDate = scr.NewDateUtc,
            NewPartOfDay = scr.NewPartOfDay,
            RequestDateTime = DateTime.MinValue 
        });
    }
}

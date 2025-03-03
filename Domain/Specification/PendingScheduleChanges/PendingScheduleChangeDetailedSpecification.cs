using Ardalis.Specification;
using Domain.Models;
using Domain.Specification.PendingScheduleChanges.Results;

namespace Domain.Specification.PendingScheduleChanges;

public class PendingScheduleChangeDetailedSpecification : Specification<PendingScheduleChange, PendingScheduleChangeDetailedResult>
{
    public PendingScheduleChangeDetailedSpecification()
    {
        Query.Select(scr => new PendingScheduleChangeDetailedResult
        {
            Id = scr.Id,
            WorkerId = scr.ScheduleToChange.ScheduleOfWorkerId,
            JobName = scr.ScheduleToChange.JobToPerform.Name,
            PreviousDate = scr.ScheduleToChange.ScheduledAtDate,
            PreviousPartOfDay = scr.ScheduleToChange.ScheduledAtPartOfDay,
            NewDate = scr.NewDateUtc,
            NewPartOfDay = scr.NewPartOfDay,
            RequestDateTime = scr.RequestDateTime,
        });
    }

    public PendingScheduleChangeDetailedSpecification(Guid workerId) : this()
    {
        Query.Where(scr => scr.ScheduleToChange.ScheduleOfWorkerId == workerId);
    }
}

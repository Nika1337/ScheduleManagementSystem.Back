

using Ardalis.Specification;
using Domain.Models;
using Domain.Specification.Schedules.Results;

namespace Domain.Specification.Schedules;

public class ScheduleDetailedSpecification : Specification<Schedule, ScheduleDetailedResult>
{
    public ScheduleDetailedSpecification()
    {
        Query.Select(s => new ScheduleDetailedResult
        {
            Id = s.Id,
            WorkerId = s.ScheduleOfWorkerId,
            JobName = s.JobToPerform.Name,
            Date = s.ScheduledAtDate,
            PartOfDay = s.ScheduledAtPartOfDay
        });
    }
}

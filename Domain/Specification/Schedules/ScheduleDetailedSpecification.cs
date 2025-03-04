

using Ardalis.Specification;
using Domain.Models;
using Domain.Specification.Schedules.Results;

namespace Domain.Specification.Schedules;

public class ScheduleDetailedSpecification : Specification<Schedule, ScheduleDetailedResult>
{
    public ScheduleDetailedSpecification(DateOnly startDate, DateOnly endDate)
    {
        Query.Where(s => s.ScheduledAtDate >= startDate && s.ScheduledAtDate < endDate);

        Query.Select(s => new ScheduleDetailedResult
        {
            Id = s.Id,
            WorkerFirstName = s.ScheduleOfWorker.FirstName,
            WorkerLastName = s.ScheduleOfWorker.LastName,
            JobName = s.JobToPerform.Name,
            Date = s.ScheduledAtDate,
            PartOfDay = s.ScheduledAtPartOfDay
        });
    }

    public ScheduleDetailedSpecification(Guid workerId, DateOnly startDate, DateOnly endDate) : this(startDate, endDate)
    {
        Query.Where(s => s.ScheduleOfWorker.Id == workerId);
    }
}

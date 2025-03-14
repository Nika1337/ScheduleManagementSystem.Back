

using Ardalis.Specification;
using Domain.Models;

namespace Domain.Specification.Schedules;
public class ScheduleByWorkerIdDatePartSpecification : SingleResultSpecification<Schedule>
{
    public ScheduleByWorkerIdDatePartSpecification(Guid workerId, DateOnly date, PartOfDay partOfDay) 
    {
        Query.Where(schedule => 
            schedule.ScheduleOfWorker.Id == workerId && 
            schedule.ScheduledAtDate == date &&
            schedule.ScheduledAtPartOfDay == partOfDay);
    }
}

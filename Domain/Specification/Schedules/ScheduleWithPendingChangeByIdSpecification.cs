﻿
using Ardalis.Specification;
using Domain.Models;

namespace Domain.Specification.Schedules;

public class ScheduleWithPendingChangeByIdSpecification : SingleResultSpecification<Schedule>
{
    public ScheduleWithPendingChangeByIdSpecification(Guid id)
    {
        Query.Where(sch => sch.Id == id);

        Query.Include(sch => sch.PendingScheduleChange);

        Query.Include(sch => sch.JobToPerform);

        Query.Include(sch => sch.ScheduleOfWorker);

        Query.AsTracking();
    }

    public ScheduleWithPendingChangeByIdSpecification(Guid id, Guid workerId) : this(id)
    {
        Query.Where(sch => sch.ScheduleOfWorker.Id == workerId);
    }
}

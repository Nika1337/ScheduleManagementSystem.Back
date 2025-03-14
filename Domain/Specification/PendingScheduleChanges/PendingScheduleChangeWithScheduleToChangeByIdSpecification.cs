﻿
using Ardalis.Specification;
using Domain.Models;

namespace Domain.Specification.PendingScheduleChanges;

public class PendingScheduleChangeWithScheduleToChangeByIdSpecification : SingleResultSpecification<PendingScheduleChange>
{
    public PendingScheduleChangeWithScheduleToChangeByIdSpecification(Guid id)
    {
        Query.Where(scr => scr.Id == id);

        Query
            .Include(scr => scr.ScheduleToChange)
            .ThenInclude(sch => sch.JobToPerform);

        Query
           .Include(scr => scr.ScheduleToChange)
           .ThenInclude(sch => sch.ScheduleOfWorker);

        Query.AsTracking();
    }
    public PendingScheduleChangeWithScheduleToChangeByIdSpecification(Guid id, Guid workerId) : this(id)
    {
        Query.Where(scr => scr.ScheduleToChange.ScheduleOfWorker.Id == workerId);
    }
}

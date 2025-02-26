﻿
using Ardalis.Specification;
using Domain.Models;

namespace Domain.Specification.Schedules;

public class ScheduleWithPendingChangeByIdSpecification : Specification<Schedule>
{
    public ScheduleWithPendingChangeByIdSpecification(Guid id)
    {
        Query.Where(sch => sch.Id == id);

        Query.Include(sch => sch.PendingScheduleChange);
    }
}

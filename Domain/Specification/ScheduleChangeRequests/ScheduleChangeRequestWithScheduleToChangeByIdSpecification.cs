
using Ardalis.Specification;
using Domain.Models;

namespace Domain.Specification.ScheduleChangeRequests;

internal class ScheduleChangeRequestWithScheduleToChangeByIdSpecification : SingleResultSpecification<ScheduleChangeRequest>
{
    public ScheduleChangeRequestWithScheduleToChangeByIdSpecification(Guid id)
    {
        Query.Where(scr => scr.Id == id);

        Query.Include(scr => scr.ScheduleToChange);
    }
}

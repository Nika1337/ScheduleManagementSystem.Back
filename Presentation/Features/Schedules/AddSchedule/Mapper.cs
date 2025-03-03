using Application.DataTransferObjects.Schedules;

namespace Schedules.AddSchedule;

internal sealed class Mapper : RequestMapper<Request, ScheduleCreateRequest>
{
    public override ScheduleCreateRequest ToEntity(Request r)
    {
        return new ScheduleCreateRequest
        {
            WorkerId = r.WorkerId,
            JobId = r.JobId,
            Date = r.Date,
            PartOfDay = r.PartOfDay,
        };
    }
}
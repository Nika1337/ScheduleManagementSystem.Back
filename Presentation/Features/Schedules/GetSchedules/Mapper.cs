using Application.DataTransferObjects.Schedules;

namespace Schedules.GetSchedules;

internal sealed class Mapper : ResponseMapper<Response, ScheduleDetailedResponse>
{
    public override Response FromEntity(ScheduleDetailedResponse e)
    {
        return new Response
        {
            Id = e.Id,
            JobName = e.JobName,
            WorkerFirstName = e.WorkerFirstName,
            WorkerLastName = e.WorkerLastName,
            Date = e.Date,
            PartOfDay = e.PartOfDay,
        };
    }
}
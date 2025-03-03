using Application.DataTransferObjects.PendingScheduleChanges;

namespace PendingScheduleChanges.GetPendingScheduleChanges;

internal sealed class Mapper : ResponseMapper<Response, PendingScheduleChangeResponse>
{
    public override Response FromEntity(PendingScheduleChangeResponse e)
    {
        return new Response
        {
            Id = e.Id,
            WorkerFirstName = e.WorkerFirstName,
            WorkerLastName = e.WorkerLastName,
            JobName = e.JobName,
            PreviousDate = e.PreviousDate,
            PreviousPartOfDay = e.PreviousPartOfDay,
            NewDate = e.NewDate,
            NewPartOfDay = e.NewPartOfDay,
            RequestDateTime = e.RequestDateTime
        };
    }

}
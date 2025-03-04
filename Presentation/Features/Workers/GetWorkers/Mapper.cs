using Application.DataTransferObjects.Workers;

namespace Workers.GetWorkers;

internal sealed class Mapper : ResponseMapper<Response, WorkerResponse>
{
    public override Response FromEntity(WorkerResponse e)
    {
        return new Response
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            StartDate = e.StartDate
        };
    }
}
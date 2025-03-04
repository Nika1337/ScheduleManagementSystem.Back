using Application.DataTransferObjects.Workers;

namespace Workers.AddWorker;

internal sealed class Mapper : RequestMapper<Request, WorkerCreateRequest>
{
    public override WorkerCreateRequest ToEntity(Request r)
    {
        return new WorkerCreateRequest
        {
            FirstName = r.FirstName,
            LastName = r.LastName,  
            Email = r.Email,
            RoleName = r.RoleName
        };
    }
}
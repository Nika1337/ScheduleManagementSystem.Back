using Application.DataTransferObjects.Jobs;

namespace Jobs.AddJob;

internal sealed class Mapper : RequestMapper<Request, JobCreateRequest>
{
    public override JobCreateRequest ToEntity(Request r)
    {
        return new JobCreateRequest
        {
            Name = r.JobName
        };
    }
}
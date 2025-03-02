using Application.DataTransferObjects.Jobs;

namespace Jobs.GetJobs;

internal sealed class Mapper : ResponseMapper<Response, JobResponse>
{
    public override Response FromEntity(JobResponse e)
    {
        return new Response
        {
            Id = e.Id,
            Name = e.Name,
        };
    }
}


using Application.DataTransferObjects.Jobs;

namespace Application.Abstractions;

public interface IJobService
{
    Task<IEnumerable<JobResponse>> GetJobsAsync();
    Task CreateJobAsync(JobCreateRequest request);
}

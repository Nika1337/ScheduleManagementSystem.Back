

using Application.Abstractions;
using Application.DataTransferObjects.Jobs;
using Domain.Abstractions;
using Domain.Models;

namespace Infrastructure.Services;

internal class JobService : IJobService
{
    private readonly IRepository<Job> _repository;

    public JobService(IRepository<Job> jobRepository)
    {
        _repository = jobRepository;
    }

    public async Task CreateJobAsync(JobCreateRequest request)
    {
        var job = new Job
        {
            Name = request.Name,
        };

        await _repository.AddAsync(job);
    }

    public async Task<IEnumerable<JobResponse>> GetJobsAsync()
    {
        var jobs = await _repository.ListAsync();

        var response = jobs.Select(job => new JobResponse
        {
            Id = job.Id,
            Name = job.Name,
        });

        return response;
    }
}

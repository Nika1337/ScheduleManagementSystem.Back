using Application.Abstractions;

namespace Jobs.GetJobs;

internal sealed class Endpoint : EndpointWithoutRequest<IEnumerable<Response>, Mapper>
{
    private readonly IJobService _jobService;

    public Endpoint(IJobService jobService)
    {
        _jobService = jobService;
    }

    public override void Configure()
    {
        Get("/jobs");
        Roles("Admin");
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var jobs = await _jobService.GetJobsAsync();

        var response = jobs.Select(job => Map.FromEntity(job));

        await SendAsync(response);
    }
}
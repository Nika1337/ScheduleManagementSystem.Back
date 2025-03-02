using Application.Abstractions;

namespace Jobs.AddJob;

internal sealed class Endpoint : EndpointWithMapper<Request, Mapper>
{
    private readonly IJobService _jobService;

    public Endpoint(IJobService jobService)
    {
        _jobService = jobService;
    }

    public override void Configure()
    {
        Post("/jobs");
        Roles("Admin");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var jobCreateRequest = Map.ToEntity(r);
        await _jobService.CreateJobAsync(jobCreateRequest);

        await SendNoContentAsync();
    }
}
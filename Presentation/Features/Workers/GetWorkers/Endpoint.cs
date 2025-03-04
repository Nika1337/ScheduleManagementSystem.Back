using Application.Abstractions;

namespace Workers.GetWorkers;

internal sealed class Endpoint : EndpointWithoutRequest<IEnumerable<Response>, Mapper>
{
    private readonly IWorkerService _workerService;

    public Endpoint(IWorkerService workerService)
    {
        _workerService = workerService;
    }

    public override void Configure()
    {
        Get("/workers");
        Roles("Admin");
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var workers = await _workerService.GetWorkersAsync();

        var response = workers.Select(worker => Map.FromEntity(worker));

        await SendAsync(response);
    }
}
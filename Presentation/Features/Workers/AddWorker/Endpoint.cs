using Application.Abstractions;

namespace Workers.AddWorker;

internal sealed class Endpoint : EndpointWithMapper<Request, Mapper>
{
    private readonly IWorkerService _workerService;

    public Endpoint(IWorkerService workerService)
    {
        _workerService = workerService;
    }

    public override void Configure()
    {
        Post("/workers");
        Roles("Admin");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var workerCreateRequest = Map.ToEntity(r);

        await _workerService.CreateWorkerAsync(workerCreateRequest);

        await SendNoContentAsync();
    }
}
using Application.Abstractions;

namespace PendingScheduleChanges.AcceptPendingScheduleChange;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IPendingScheduleChangeService _pendingScheduleChangeService;

    public Endpoint(IPendingScheduleChangeService pendingScheduleChangeService)
    {
        _pendingScheduleChangeService = pendingScheduleChangeService;
    }

    public override void Configure()
    {
        Post("/pending-schedule-changes/{Id}/accept");
        Description(x => x.Accepts<Request>());
        Roles("Admin");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        await _pendingScheduleChangeService.AcceptPendingScheduleChange(r.Id);

        await SendNoContentAsync();
    }
}
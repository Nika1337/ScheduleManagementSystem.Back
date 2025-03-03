using Application.Abstractions;

namespace PendingScheduleChanges.RejectPendingScheduleChange;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IPendingScheduleChangeService _pendingScheduleChangeService;

    public Endpoint(IPendingScheduleChangeService pendingScheduleChangeService)
    {
        _pendingScheduleChangeService = pendingScheduleChangeService;
    }

    public override void Configure()
    {
        Post("pending-schedule-changes/{Id}/reject");
        Roles("Admin");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        await _pendingScheduleChangeService.RejectPendingScheduleChange(r.Id);

        await SendNoContentAsync();
    }
}
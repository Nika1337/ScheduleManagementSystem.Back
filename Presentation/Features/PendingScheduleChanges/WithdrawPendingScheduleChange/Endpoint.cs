using Application.Abstractions;
using System.Security.Claims;

namespace PendingScheduleChange.WithdrawPendingScheduleChange;

internal sealed class Endpoint : Endpoint<Request>
{

    private readonly IPendingScheduleChangeService _pendingScheduleChangeService;

    public Endpoint(IPendingScheduleChangeService pendingScheduleChangeService)
    {
        _pendingScheduleChangeService = pendingScheduleChangeService;
    }

    public override void Configure()
    {
        Post("/pending-schedule-changes/{Id}/withdraw");
        Description(x => x.Accepts<Request>());
        Roles("Worker");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var employeeIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var employeeId = Guid.Parse(employeeIdClaim);

        await _pendingScheduleChangeService.WithdrawPendingScheduleChange(r.Id, employeeId);

        await SendNoContentAsync();
    }
}
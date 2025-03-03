using Application.Abstractions;
using Application.DataTransferObjects.PendingScheduleChanges;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PendingScheduleChanges.GetPendingScheduleChanges;

internal sealed class Endpoint : EndpointWithoutRequest<IEnumerable<Response>, Mapper>
{
    private readonly IPendingScheduleChangeService _pendingScheduleChangeService;

    public Endpoint(IPendingScheduleChangeService pendingScheduleChangeService)
    {
        _pendingScheduleChangeService = pendingScheduleChangeService;
    }

    public override void Configure()
    {
        Get("/pending-schedule-changes");
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        IEnumerable<PendingScheduleChangeResponse> scheduleChanges;

        if (userRole == "Admin")
        {
            scheduleChanges = await _pendingScheduleChangeService.GetPendingScheduleChangesAsync();
        }
        else if (userRole == "Worker" && Guid.TryParse(userId, out var workerId))
        {
            scheduleChanges = await _pendingScheduleChangeService.GetPendingScheduleChangesAsync(workerId);
        }
        else
        {
            await SendForbiddenAsync(c);
            return;
        }

        var response = scheduleChanges.Select(sc => Map.FromEntity(sc));

        await SendAsync(response);
    }
}

using Application.Abstractions;
using Application.DataTransferObjects.Schedules;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Schedules.GetScheduleChangeEvents;

internal sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IScheduleNotificationService _notificationService;

    public Endpoint(IScheduleNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public override void Configure()
    {
        Get("schedules/change-events");
        Roles("Worker");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var userId = Guid.Parse(userIdClaim);

        await SendEventStreamAsync(
                eventName: "schedule-changed",
                eventStream: GetDataStream(userId, c),
                cancellation: c);
    }

    private async IAsyncEnumerable<ScheduleChangeEvent> GetDataStream(
            Guid userId,
            [EnumeratorCancellation] CancellationToken ct)
    {
        await foreach (var change in _notificationService.SubscribeWorkerChanges(userId, ct))
        {
            yield return change;
        }
    }
}
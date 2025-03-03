using Application.Abstractions;
using Application.DataTransferObjects.PendingScheduleChanges;
using Application.DataTransferObjects.Schedules;
using System.Security.Claims;

namespace Schedules.GetSchedules;

internal sealed class Endpoint : Endpoint<Request, IEnumerable<Response>, Mapper>
{
    private readonly IScheduleService _scheduleService;

    public Endpoint(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    public override void Configure()
    {
        Get("/schedules");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        IEnumerable<ScheduleDetailedResponse> schedules;

        if (userRole == "Admin")
        {
            schedules = await _scheduleService.GetSchedulesAsync(r.StartDate, r.EndDate);
        }
        else if (userRole == "Worker" && Guid.TryParse(userId, out var workerId))
        {
            schedules = await _scheduleService.GetSchedulesForWorkerAsync(workerId, r.StartDate, r.EndDate);
        }
        else
        {
            await SendForbiddenAsync();
            return;
        }

        var response = schedules.Select(Map.FromEntity);

        await SendAsync(response);
    }
}
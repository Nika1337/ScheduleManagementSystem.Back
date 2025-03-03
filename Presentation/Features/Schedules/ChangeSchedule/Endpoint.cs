using Application.Abstractions;
using Application.DataTransferObjects.Schedules;
using System.Security.Claims;

namespace Schedules.ChangeSchedule;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IScheduleService _scheduleService;

    public Endpoint(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    public override void Configure()
    {
        Patch("/schedules/{Id}");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


        if (userRole == "Admin")
        {
            var request = new ScheduleChangeRequest
            {
                Id = r.Id,
                Date = r.Date,
                PartOfDay = r.PartOfDay
            };
            await _scheduleService.ChangeScheduleAsync(request);
        }
        else if (userRole == "Worker" && Guid.TryParse(userId, out var workerId))
        {
            var request = new ScheduleChangeByWorkerRequest
            {
                WorkerId = workerId,
                Id = r.Id,
                Date = r.Date,
                PartOfDay = r.PartOfDay
            };
            await _scheduleService.RequestScheduleChangeAsync(request);
        }
        else
        {
            await SendForbiddenAsync();
            return;
        }


        await SendNoContentAsync();
    }
}
using Application.Abstractions;

namespace Schedules.AddSchedule;

internal sealed class Endpoint : EndpointWithMapper<Request, Mapper>
{
    private readonly IScheduleService _scheduleService;

    public Endpoint(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    public override void Configure()
    {
        Post("/schedules");
        Roles("Admin");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var scheduleCreateRequest = Map.ToEntity(r);

        await _scheduleService.CreateScheduleAsync(scheduleCreateRequest);

        await SendNoContentAsync();
    }
}
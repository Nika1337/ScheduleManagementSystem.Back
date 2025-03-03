

namespace Application.DataTransferObjects.Schedules;
public record ScheduleChangeByWorkerRequest : ScheduleChangeRequest
{
    public required Guid WorkerId { get; init; }
}

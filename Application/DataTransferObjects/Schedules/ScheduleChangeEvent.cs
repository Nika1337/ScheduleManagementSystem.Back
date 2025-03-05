
namespace Application.DataTransferObjects.Schedules;
public record ScheduleChangeEvent
{
    public required Guid WorkerId { get; init; }
    public required string Message { get; init; }
}

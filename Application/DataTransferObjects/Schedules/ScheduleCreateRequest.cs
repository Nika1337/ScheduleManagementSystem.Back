

using Domain.Models;

namespace Application.DataTransferObjects.Schedules;

public record ScheduleCreateRequest
{
    public required Guid WorkerId { get; init; }
    public required Guid JobId { get; init; }
    public required DateOnly Date { get; init; }
    public required PartOfDay PartOfDay { get; init; }
}

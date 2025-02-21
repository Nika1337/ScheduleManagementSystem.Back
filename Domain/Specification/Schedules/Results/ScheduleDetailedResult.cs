

using Domain.Models;

namespace Domain.Specification.Schedules.Results;

public record ScheduleDetailedResult
{
    public required Guid Id { get; init; }
    public required Guid WorkerId { get; init; }
    public required string JobName { get; init; }
    public required DateOnly Date { get; init; }
    public required PartOfDay PartOfDay { get; init; }
}

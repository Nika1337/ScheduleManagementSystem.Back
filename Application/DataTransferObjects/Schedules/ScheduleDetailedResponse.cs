

using Domain.Models;

namespace Application.DataTransferObjects.Schedules;

public record ScheduleDetailedResponse
{
    public required Guid Id { get; init; }
    public required string JobName { get; init; }
    public required string WorkerFirstName { get; init; }
    public required string WorkerLastName { get; init; }
    public required DateOnly Date { get; init; }
    public required PartOfDay PartOfDay { get; init; }
}

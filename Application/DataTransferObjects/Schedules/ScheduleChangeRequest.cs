

using Domain.Models;

namespace Application.DataTransferObjects.Schedules;

public record ScheduleChangeRequest
{
    public required Guid Id { get; init; }
    public required DateOnly Date { get; init; }
    public required PartOfDay PartOfDay { get; init; }
}

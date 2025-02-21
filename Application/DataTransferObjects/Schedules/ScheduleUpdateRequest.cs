

using Domain.Models;

namespace Application.DataTransferObjects.Schedules;

public record ScheduleUpdateRequest
{
    public Guid Id { get; init; }
    public DateOnly Date { get; init; }
    public PartOfDay PartOfDay { get; init; }
}

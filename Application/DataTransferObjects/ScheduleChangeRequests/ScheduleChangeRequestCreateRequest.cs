

using Domain.Models;

namespace Application.DataTransferObjects.ScheduleChangeRequests;

public record ScheduleChangeRequestCreateRequest
{
    public required Guid ScheduleId { get; init; }
    public required DateOnly NewDate { get; init; }
    public required PartOfDay PartOfDay { get; init; }
}



using Domain.Models;

namespace Domain.Specification.ScheduleChangeRequests.Results;

public record ScheduleChangeRequestDetailedResult
{
    public required Guid Id { get; init; }
    public required Guid WorkerId { get; init; }
    public required string JobName { get; init; }
    public required DateOnly PreviousDate { get; init; }
    public required PartOfDay PreviousPartOfDay {  get; init; }
    public required DateOnly NewDate { get; init; }
    public required PartOfDay NewPartOfDay { get; init; }
    public required DateTime RequestDateTime { get; init; }
}

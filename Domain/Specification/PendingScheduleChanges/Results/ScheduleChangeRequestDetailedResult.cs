

using Domain.Models;

namespace Domain.Specification.PendingScheduleChanges.Results;

public record PendingScheduleChangeDetailedResult
{
    public required Guid Id { get; init; }
    public required string WorkerFirstName { get; init; }
    public required string WorkerLastName { get; init; }
    public required string JobName { get; init; }
    public required DateOnly PreviousDate { get; init; }
    public required PartOfDay PreviousPartOfDay {  get; init; }
    public required DateOnly NewDate { get; init; }
    public required PartOfDay NewPartOfDay { get; init; }
    public required DateTime RequestDateTime { get; init; }
}

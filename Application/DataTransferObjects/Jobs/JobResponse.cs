namespace Application.DataTransferObjects.Jobs;

public record JobResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}

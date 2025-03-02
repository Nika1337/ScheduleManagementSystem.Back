namespace Jobs.GetJobs;

internal sealed class Response
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}

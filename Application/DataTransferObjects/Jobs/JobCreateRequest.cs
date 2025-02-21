

namespace Application.DataTransferObjects.Jobs;

public record JobCreateRequest
{
    public required string Name { get; init; }
}

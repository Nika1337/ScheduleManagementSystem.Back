

namespace Application.DataTransferObjects.Workers;
public record WorkerProfileResponse
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
}

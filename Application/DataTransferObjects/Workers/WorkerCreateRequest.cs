

namespace Application.DataTransferObjects.Workers;

public record WorkerCreateRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string RoleName { get; init; }
}

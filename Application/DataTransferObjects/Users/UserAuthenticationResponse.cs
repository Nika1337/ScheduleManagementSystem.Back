

namespace Application.DataTransferObjects.Users;
public record UserAuthenticationResponse
{
    public required Guid UserId { get; init; }
    public required string RoleName { get; init; }
    public required string Email { get; init; }
}

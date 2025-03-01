

namespace Application.DataTransferObjects.Employees;
public record EmployeeAuthenticationResponse
{
    public required Guid EmployeeId { get; init; }
    public required string RoleName { get; init; }
}

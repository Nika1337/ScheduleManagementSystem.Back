

namespace Application.DataTransferObjects.Employees;

public record EmployeeUpdateByAdminRequest
{
    public required Guid Id { get; init; }
    public required string RoleName { get; init; }
}

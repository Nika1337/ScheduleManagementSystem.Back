

namespace Application.DataTransferObjects.Employees;

public record EmployeeResponse
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string StartDate { get; init; }
    public required string RoleName { get; init; }
}

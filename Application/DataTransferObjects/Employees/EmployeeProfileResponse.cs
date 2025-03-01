

namespace Application.DataTransferObjects.Employees;
public record EmployeeProfileResponse
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
}



namespace Application.DataTransferObjects.Employees;

public record EmployeeProfileUpdateRequest
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public string? ProfilePictureBase64 { get; init; }
}

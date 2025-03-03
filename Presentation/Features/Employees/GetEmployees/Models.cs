namespace Employees.GetEmployees;


internal sealed class Response
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required DateTime StartDate { get; init; }
    public required string RoleName { get; init; }
}

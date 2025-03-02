using Application.Abstractions;

namespace Employees.GetEmployees;

internal sealed class Endpoint : EndpointWithoutRequest<IEnumerable<Response>, Mapper>
{
    private readonly IEmployeeService _employeeService;

    public Endpoint(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public override void Configure()
    {
        Get("/employees");
        Roles("Admin");
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var employees = await _employeeService.GetEmployeesAsync();

        var response = employees.Select(employee => Map.FromEntity(employee));

        await SendAsync(response);
    }
}
using Application.Abstractions;

namespace Employees.UpdateEmployee;

internal sealed class Endpoint : EndpointWithMapper<Request, Mapper>
{
    private readonly IEmployeeService _employeeService;

    public Endpoint(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public override void Configure()
    {
        Patch("employees/{EmployeeId:guid}");
        Roles("Admin");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var employeeUpdateRequest = Map.ToEntity(r);

        await _employeeService.UpdateEmployeeAsync(employeeUpdateRequest);

        await SendNoContentAsync();
    }
}
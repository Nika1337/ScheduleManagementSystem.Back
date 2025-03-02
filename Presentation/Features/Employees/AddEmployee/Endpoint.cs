using Application.Abstractions;

namespace Employees.AddEmployee;

internal sealed class Endpoint : EndpointWithMapper<Request, Mapper>
{
    private readonly IEmployeeService _employeeService;

    public Endpoint(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public override void Configure()
    {
        Post("/employees");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var employeeCreateRequest = Map.ToEntity(r);

        await _employeeService.CreateEmployeeAsync(employeeCreateRequest);

        await SendNoContentAsync();
    }
}
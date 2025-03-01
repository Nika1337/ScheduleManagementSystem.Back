using Application.Abstractions;
using System.Security.Claims;

namespace Profile.GetProfile;

internal sealed class Endpoint : EndpointWithoutRequest<Response, Mapper>
{
    private readonly IEmployeeService _employeeService;

    public Endpoint(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public override void Configure()
    {
        Post("/profile");
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var employeeIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var employeeId = Guid.Parse(employeeIdClaim.Value);

        var employee = await _employeeService.GetEmployeeProfileAsync(employeeId);

        var response = Map.FromEntity(employee);

        await SendAsync(response);
    }
}
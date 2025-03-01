using Application.Abstractions;
using Application.DataTransferObjects.Employees;
using System.Security.Claims;

namespace Profile.UpdateProfile;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IEmployeeService _employeeService;

    public Endpoint(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public override void Configure()
    {
        Patch("/profile");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var employeeIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var employeeId = Guid.Parse(employeeIdClaim.Value);

        var updateRequest = new EmployeeProfileUpdateRequest 
        {
            Id = employeeId,
            FirstName = r.FirstName,
            LastName = r.LastName,
            Email = r.Email
        };


        await _employeeService.UpdateEmployeeAsync(updateRequest);
    }

}
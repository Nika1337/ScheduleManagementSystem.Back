using Application.Abstractions;
using Domain.Exceptions;
using System.Security.Claims;

namespace Profile.ChangePassword;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IEmployeeAuthenticationService _employeeAuthenticationService;

    public Endpoint(IEmployeeAuthenticationService employeeAuthenticationService)
    {
        _employeeAuthenticationService = employeeAuthenticationService;
    }

    public override void Configure()
    {
        Post("/profile/change-password");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var employeeIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var employeeId = Guid.Parse(employeeIdClaim);

        try
        {
            await _employeeAuthenticationService.ChangePasswordAsync(employeeId, r.CurrentPassword, r.NewPassword);
            await SendNoContentAsync();
        } 
        catch (PasswordIncorrectException)
        {
            await SendAsync(new { error = "Current password is incorrect." }, 400);
        }
        catch (PasswordStructureValidationException e)
        {
            await SendAsync(new { error = e.Message }, 400);
        }
    }
}
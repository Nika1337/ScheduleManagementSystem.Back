using Application.Abstractions;
using Domain.Exceptions;
using System.Security.Claims;

namespace Auth.ChangePassword;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IUserAuthenticationService _userAuthenticationService;

    public Endpoint(IUserAuthenticationService userAuthenticationService)
    {
        _userAuthenticationService = userAuthenticationService;
    }

    public override void Configure()
    {
        Post("/change-password");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var userId = Guid.Parse(userIdClaim);

        try
        {
            await _userAuthenticationService.ChangePasswordAsync(userId, r.CurrentPassword, r.NewPassword);
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
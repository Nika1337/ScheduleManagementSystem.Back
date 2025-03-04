using Application.Abstractions;
using System.Security.Claims;

namespace Auth.ChangeEmail;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IUserAuthenticationService _userAuthenticationService;

    public Endpoint(IUserAuthenticationService userAuthenticationService)
    {
        _userAuthenticationService = userAuthenticationService;
    }

    public override void Configure()
    {
        Post("/change-email");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var userId = Guid.Parse(userIdClaim);

        await _userAuthenticationService.ChangeEmailAsync(userId, r.NewEmail);
        await SendNoContentAsync();
    }
}
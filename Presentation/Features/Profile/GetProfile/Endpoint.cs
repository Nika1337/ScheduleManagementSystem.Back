using Application.Abstractions;
using System.Security.Claims;

namespace Profile.GetProfile;

internal sealed class Endpoint : EndpointWithoutRequest<Response, Mapper>
{
    private readonly IWorkerService _userService;

    public Endpoint(IWorkerService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/profile");
        Roles("Worker");
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var userId = Guid.Parse(userIdClaim);

        var user = await _userService.GetWorkerProfileAsync(userId);

        var response = Map.FromEntity(user);

        await SendAsync(response);
    }
}
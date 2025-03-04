using Application.Abstractions;
using Application.DataTransferObjects.Workers;
using System.Security.Claims;

namespace Profile.UpdateProfile;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IWorkerService _userService;

    public Endpoint(IWorkerService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Patch("/profile");
        Roles("Worker");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var userId = Guid.Parse(userIdClaim);

        var updateRequest = new WorkerProfileUpdateRequest 
        {
            Id = userId,
            FirstName = r.FirstName,
            LastName = r.LastName,
            Email = r.Email
        };


        await _userService.UpdateWorkerAsync(updateRequest);
        await SendNoContentAsync();
    }

}
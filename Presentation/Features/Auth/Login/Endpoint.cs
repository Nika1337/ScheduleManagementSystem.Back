
using Application.Abstractions;
using Application.DataTransferObjects.Users;
using Domain.Exceptions;
using System.Security.Claims;

namespace Auth.Login;

internal sealed class Endpoint : Endpoint<Request, Response>
{
    private readonly IUserAuthenticationService _userAuthenticationService;

    public Endpoint(IUserAuthenticationService userAuthenticationService)
    {
        _userAuthenticationService = userAuthenticationService;
    }

    public override void Configure()
    {
        Post("/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        UserAuthenticationResponse userAuthenticationResult;
        try
        {
            userAuthenticationResult = await _userAuthenticationService.AuthenticateAsync(r.Email, r.Password);
        }
        catch (PasswordIncorrectException)
        {
            await SendUnauthorizedAsync();
            return;
        }

        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, userAuthenticationResult.UserId.ToString()),
            new(ClaimTypes.Role, userAuthenticationResult.RoleName),
            new(ClaimTypes.Email, userAuthenticationResult.Email)
        };

        var jwtToken = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = Config["JwtSigningKey"]!;
            o.ExpireAt = DateTime.Now.AddHours(1);
            o.User.Claims.Add(claims);
        });

        var response = new Response
        {
            Token = jwtToken
        };

        await SendAsync(response);
    }
}
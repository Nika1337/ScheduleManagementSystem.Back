
using Application.Abstractions;
using Application.DataTransferObjects.Employees;
using Domain.Exceptions;
using System.Security.Claims;

namespace Public.Login;

internal sealed class Endpoint : Endpoint<Request, Response>
{
    private readonly IEmployeeAuthenticationService _employeeAuthenticationService;

    public Endpoint(IEmployeeAuthenticationService employeeAuthenticationService)
    {
        _employeeAuthenticationService = employeeAuthenticationService;
    }

    public override void Configure()
    {
        Post("/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        EmployeeAuthenticationResponse employeeAuthenticationResult;
        try
        {
            employeeAuthenticationResult = await _employeeAuthenticationService.AuthenticateAsync(r.Email, r.Password);
        }
        catch (PasswordIncorrectException)
        {
            await SendUnauthorizedAsync();
            return;
        }

        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, employeeAuthenticationResult.EmployeeId.ToString()),
            new(ClaimTypes.Role, employeeAuthenticationResult.RoleName)
        };

        var jwtToken = JwtBearer.CreateToken( o =>
        {
            o.SigningKey = Config["JwtSigningKey"]!;
            o.ExpireAt = DateTime.UtcNow.AddHours(1);
            o.User.Claims.Add(claims);
        });

        var response = new Response
        {
            Token = jwtToken
        };

        await SendAsync(response);
    }
}
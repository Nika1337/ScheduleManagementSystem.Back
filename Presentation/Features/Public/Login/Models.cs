
using FluentValidation;

namespace Public.Login;

internal sealed class Request
{
    public required string Email { get; init; }
    public required string Password { get; init; }

    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(r => r.Password)
                .NotEmpty();
        }
    }
}

internal sealed class Response
{
    public required string Token { get; init; }
}

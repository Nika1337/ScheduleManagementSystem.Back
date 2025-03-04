using FluentValidation;

namespace Auth.ChangeEmail;

internal sealed class Request
{
    public required string NewEmail { get; init; }

    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.NewEmail)
                .NotEmpty()
                .Length(1, 250)
                .EmailAddress();
        }
    }
}

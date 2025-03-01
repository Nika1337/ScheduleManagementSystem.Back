using FluentValidation;

namespace Profile.UpdateProfile;

internal sealed class Request
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }

    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.FirstName)
                .Length(1, 250);

            RuleFor(r => r.LastName)
                .Length(1, 250);

            RuleFor(r => r.Email)
                .Length(1, 250)
                .EmailAddress();
        }
    }
}
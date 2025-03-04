using FluentValidation;

namespace Profile.UpdateProfile;

internal sealed class Request
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.FirstName)
                .NotEmpty()
                .Length(1, 250);

            RuleFor(r => r.LastName)
                .NotEmpty()
                .Length(1, 250);
        }
    }
}
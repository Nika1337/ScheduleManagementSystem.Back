using FluentValidation;

namespace Employees.AddEmployee;

internal sealed class Request
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string RoleName { get; init; }

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

            RuleFor(r => r.Email)
                .NotEmpty()
                .Length(1, 250)
                .EmailAddress();

            RuleFor(r => r.RoleName)
                .NotEmpty();
        }
    }
}
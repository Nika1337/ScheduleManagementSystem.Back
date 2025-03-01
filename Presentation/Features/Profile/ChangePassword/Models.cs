using FluentValidation;

namespace Profile.ChangePassword;

internal sealed class Request
{
    public required string CurrentPassword { get; init; }
    public required string NewPassword { get; init; }

    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.")
                .Length(8, 64).WithMessage("Current password must be between 8 and 64 characters.");

            RuleFor(r => r.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .Length(8, 64).WithMessage("New password must be between 8 and 64 characters.")
                .NotEqual(r => r.CurrentPassword).WithMessage("New password must be different from the current password.");

        }
    }
}
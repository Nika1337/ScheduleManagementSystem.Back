using FluentValidation;

namespace Jobs.AddJob;

internal sealed class Request
{
    public required string JobName { get; init; }

    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.JobName)
                .NotEmpty()
                .Length(2, 80).WithMessage("Job name must be between 2 and 80 characters");
        }
    }
}
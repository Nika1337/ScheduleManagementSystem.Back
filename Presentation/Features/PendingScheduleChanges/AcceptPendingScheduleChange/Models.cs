

using FluentValidation;

namespace PendingScheduleChanges.AcceptPendingScheduleChange;

internal sealed class Request
{
    public required Guid Id { get; init; }

    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Id)
                .NotEmpty();
        }
    }
}
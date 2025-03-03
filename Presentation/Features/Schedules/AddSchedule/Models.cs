using Domain.Models;
using FluentValidation;

namespace Schedules.AddSchedule;

internal sealed class Request
{
    public required Guid WorkerId { get; init; }
    public required Guid JobId { get; init; }
    public required DateOnly Date {  get; init; }
    public required PartOfDay PartOfDay { get; init; }

    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.WorkerId)
                .NotEmpty();

            RuleFor(r => r.JobId)
                .NotEmpty();

            RuleFor(r => r.Date)
                .NotEmpty()
                .GreaterThan(DateOnly.FromDateTime(DateTime.Now.AddDays(-1)));

            RuleFor(r => r.PartOfDay)
                .NotEmpty();
        }
    }
}
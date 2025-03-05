using Domain.Models;
using FluentValidation;

namespace Schedules.GetSchedules;

internal sealed class Request
{
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }

    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.StartDate)
                .NotEmpty()
                .LessThanOrEqualTo(r => r.EndDate).WithMessage("Start date must be less than or equal to end date.");

            RuleFor(r => r.EndDate)
                .NotEmpty();
        }
    }
}

internal sealed class Response
{
    public required Guid Id { get; init; }
    public required string JobName { get; init; }
    public required string WorkerFirstName { get; init; }
    public required string WorkerLastName { get; init; }
    public required DateOnly Date { get; init; }
    public required PartOfDay PartOfDay { get; init; }
}

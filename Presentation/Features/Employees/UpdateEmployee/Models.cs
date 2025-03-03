using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Employees.UpdateEmployee;

internal sealed class Request
{
    public required Guid EmployeeId { get; init; }
    public required string RoleName { get; init; }

    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.EmployeeId)
                .NotEmpty();

            RuleFor(r => r.RoleName)
                .NotEmpty();
        }
    }
}
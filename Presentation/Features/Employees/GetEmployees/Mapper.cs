using Application.DataTransferObjects.Employees;

namespace Employees.GetEmployees;

internal sealed class Mapper : ResponseMapper<Response, EmployeeResponse>
{
    public override Response FromEntity(EmployeeResponse e)
    {
        return new Response
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            StartDateUtc = e.StartDateUtc,
            RoleName = e.RoleName,
        };
    }
}
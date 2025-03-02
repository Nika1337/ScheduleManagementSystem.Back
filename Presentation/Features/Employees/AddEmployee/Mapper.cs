using Application.DataTransferObjects.Employees;

namespace Employees.AddEmployee;

internal sealed class Mapper : RequestMapper<Request, EmployeeCreateRequest>
{
    public override EmployeeCreateRequest ToEntity(Request r)
    {
        return new EmployeeCreateRequest
        {
            FirstName = r.FirstName,
            LastName = r.LastName,  
            Email = r.Email,
            RoleName = r.RoleName
        };
    }
}
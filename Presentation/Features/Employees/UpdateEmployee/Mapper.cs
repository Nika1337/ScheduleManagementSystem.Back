using Application.DataTransferObjects.Employees;

namespace Employees.UpdateEmployee;

internal sealed class Mapper : RequestMapper<Request, EmployeeUpdateByAdminRequest>
{
    public override EmployeeUpdateByAdminRequest ToEntity(Request r)
    {
        return new EmployeeUpdateByAdminRequest
        {
            Id = r.EmployeeId,
            RoleName = r.RoleName,
        };
    }
}
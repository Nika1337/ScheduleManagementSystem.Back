using Application.DataTransferObjects.Employees;

namespace Profile;

internal sealed class Mapper : ResponseMapper<Response, EmployeeProfileResponse>
{
    public override Response FromEntity(EmployeeProfileResponse e)
    {
        return new Response
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
        };
    }
}
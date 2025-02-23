

using Application.DataTransferObjects.Employees;

namespace Application.Abstractions;

public interface IEmployeeService
{
    Task CreateEmployeeAsync(EmployeeCreateRequest request);
    Task<IEnumerable<EmployeeResponse>> GetEmployeesAsync();
    Task UpdateEmployeeAsync(EmployeeUpdateByAdminRequest request);
    Task UpdateEmployeeAsync(EmployeeProfileUpdateRequest request);
}

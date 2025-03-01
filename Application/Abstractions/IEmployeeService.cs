

using Application.DataTransferObjects.Employees;

namespace Application.Abstractions;

public interface IEmployeeService
{
    Task CreateEmployeeAsync(EmployeeCreateRequest request);
    Task<EmployeeProfileResponse> GetEmployeeProfileAsync(Guid id);
    Task<IEnumerable<EmployeeResponse>> GetEmployeesAsync();
    Task UpdateEmployeeAsync(EmployeeUpdateByAdminRequest request);
    Task UpdateEmployeeAsync(EmployeeProfileUpdateRequest request);
}

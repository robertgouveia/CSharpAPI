using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IEmployeeService
{
    IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
    EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    EmployeeDto CreateEmployee(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);
    void DeleteEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    void UpdateEmployee(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool compTrackChanges, bool empTrackChanges);
}
using System.Collections;
using System.Dynamic;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<(IEnumerable<ExpandoObject> employees, MetaData metaData)> GetEmployees(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<EmployeeDto> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    Task<EmployeeDto> CreateEmployee(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);
    Task DeleteEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    Task UpdateEmployee(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool compTrackChanges, bool empTrackChanges);
    Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatch(Guid companyId, Guid employeeId,
        bool compTrackChanges, bool empTrackChanges);

    Task SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employee);
}
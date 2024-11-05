using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

// More entity specific
public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetEmployees(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateEmployee(Guid companyId, Employee employee); // No DB changes
    void DeleteEmployee(Employee employee);
}
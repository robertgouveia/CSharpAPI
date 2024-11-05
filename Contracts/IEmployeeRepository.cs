using Entities.Models;

namespace Contracts;

// More entity specific
public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployees(Guid companyId, bool trackChanges);
    Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateEmployee(Guid companyId, Employee employee); // No DB changes
    void DeleteEmployee(Employee employee);
}
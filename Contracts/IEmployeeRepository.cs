using Entities.Models;

namespace Contracts;

// More entity specific
public interface IEmployeeRepository
{
    IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges);
    Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
}
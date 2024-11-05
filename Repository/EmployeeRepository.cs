using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

// Inherits the repository base methods but in relation to Employee
public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

    public async Task<IEnumerable<Employee>> GetEmployees(Guid companyId, bool trackChanges) =>
        await FindByCondition(e => e.Company!.Id == companyId, trackChanges).OrderBy(e => e.Name).ToListAsync();

    public async Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges) =>
        await FindByCondition(e => e.Company!.Id == companyId && e.Id == employeeId, trackChanges).FirstOrDefaultAsync();

    public void CreateEmployee(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee) => Delete(employee);
}
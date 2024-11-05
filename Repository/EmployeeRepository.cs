using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository;

// Inherits the repository base methods but in relation to Employee
public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

    public async Task<PagedList<Employee>> GetEmployees(Guid companyId, EmployeeParameters employeeParameters,
        bool trackChanges)
    {
        var employees = await FindByCondition(e => e.Company!.Id == companyId && e.Age >= employeeParameters.MinAge && e.Age <= employeeParameters.MaxAge, trackChanges)
            .OrderBy(e => e.Name)
            .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
            .Take(employeeParameters.PageSize)
            .ToListAsync();

        // Added a total count on repository level
        var count = await FindByCondition(e => e.CompanyId == companyId, trackChanges).CountAsync();

        return new PagedList<Employee>(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
    }

    public async Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges) =>
        await FindByCondition(e => e.Company!.Id == companyId && e.Id == employeeId, trackChanges).FirstOrDefaultAsync();

    public void CreateEmployee(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee) => Delete(employee);
}
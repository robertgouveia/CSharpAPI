using Contracts;
using Entities.Models;

namespace Repository;

// Inherits the repository base methods but in relation to Employee
public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

    public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges) =>
        FindByCondition(e => e.Company!.Id == companyId, trackChanges).OrderBy(e => e.Name).ToList();
}
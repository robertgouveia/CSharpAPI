using Contracts;
using Entities.Models;

namespace Repository;

// Inherits the repository base methods but in relation to Employee
public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }
}
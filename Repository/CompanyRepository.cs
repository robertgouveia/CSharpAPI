using Contracts;
using Entities.Models;

namespace Repository;

// Inherits all the repository methods and inputs Company
public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }
}
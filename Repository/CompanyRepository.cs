using Contracts;
using Entities.Models;

namespace Repository;

// Inherits all the repository methods and inputs Company
public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

    // Since we inherit FindAll we can call it
    public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
        FindAll(trackChanges).OrderBy(c => c.Name).ToList();

    public Company GetCompany(Guid companyId, bool trackChanges) =>
        FindByCondition(c => c.Id == companyId, trackChanges).SingleOrDefault();
}
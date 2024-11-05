using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

// Inherits all the repository methods and inputs Company
public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

    // Since we inherit FindAll we can call it
    public async Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges) =>
        await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();

    public async Task<IEnumerable<Company>> GetCompanies(IEnumerable<Guid> companyIds, bool trackChanges) =>
        await FindByCondition(c => companyIds.Contains(c.Id), trackChanges).ToListAsync();

    public async Task<Company> GetCompany(Guid companyId, bool trackChanges) =>
        await FindByCondition(c => c.Id == companyId, trackChanges).OrderBy(c => c.Name).SingleOrDefaultAsync();

    public void CreateCompany(Company company) => Create(company);

    public void DeleteCompany(Company company) => Delete(company);
}
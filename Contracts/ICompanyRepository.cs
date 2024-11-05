using Entities.Models;

namespace Contracts;

// More entity specific
public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges);
    Task<IEnumerable<Company>> GetCompanies(IEnumerable<Guid> companyIds, bool trackChanges);
    Task<Company> GetCompany(Guid companyId, bool trackChanges);
    void CreateCompany(Company company); // No DB changes so Async is not needed
    void DeleteCompany(Company company);
}
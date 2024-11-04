using Entities.Models;

namespace Contracts;

// More entity specific
public interface ICompanyRepository
{
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
    IEnumerable<Company> GetCompanies(IEnumerable<Guid> companyIds, bool trackChanges);
    Company GetCompany(Guid companyId, bool trackChanges);
    void CreateCompany(Company company);
}
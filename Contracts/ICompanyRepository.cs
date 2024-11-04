using Entities.Models;

namespace Contracts;

// More entity specific
public interface ICompanyRepository
{
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
}
using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
    IEnumerable<CompanyDto> GetCompanies(IEnumerable<Guid> companyIds, bool trackChanges);
    CompanyDto GetCompany(Guid companyId, bool trackChanges);
    CompanyDto Create(CompanyForCreationDto company);
    (IEnumerable<CompanyDto> companyDtos, string companyIds) CreateCompanies(IEnumerable<CompanyForCreationDto> companies);
    void DeleteCompany(Guid companyId, bool trackChanges);
    void UpdateCompany(Guid companyId, CompanyForUpdateDto company, bool trackChanges);
}
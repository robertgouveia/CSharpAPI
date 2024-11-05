using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompanies(bool trackChanges);
    Task<IEnumerable<CompanyDto>> GetCompanies(IEnumerable<Guid> companyIds, bool trackChanges);
    Task<CompanyDto> GetCompany(Guid companyId, bool trackChanges);
    Task<CompanyDto> Create(CompanyForCreationDto company);
    Task<(IEnumerable<CompanyDto> companyDtos, string companyIds)> CreateCompanies(IEnumerable<CompanyForCreationDto> companies);
    Task DeleteCompany(Guid companyId, bool trackChanges);
    Task UpdateCompany(Guid companyId, CompanyForUpdateDto company, bool trackChanges);
}
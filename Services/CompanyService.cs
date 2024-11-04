using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Services;

internal sealed class CompanyService : ICompanyService
{
    // Holds access to the repository / logger
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    // Services act as a middle layer between repositories and business logic
    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        var companies = _repository.Company.GetAllCompanies(trackChanges);
        
        // Data Transfer Object - Manual Mapping
        //var companiesDto = companies.Select(c => new CompanyDto(c.Id, c.Name ?? "", string.Join(' ', c.Address, c.Country))); 
            
        //Using an Auto Mapper - turning companies into IEnumerable<CompanyDto>
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
                
        return companiesDto;
        // We do not need a try catch block due to the exception handler picking up the error
    }

    public CompanyDto GetCompany(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        // Check company is null

        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }
}
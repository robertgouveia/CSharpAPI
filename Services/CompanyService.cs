using AutoMapper;
using Contracts;
using Entities.Exceptions;
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

    public IEnumerable<CompanyDto> GetCompanies(IEnumerable<Guid> companyIds, bool trackChanges)
    {
        if (companyIds is null) throw new IdParamsBadRequestException();

        var companies = _repository.Company.GetCompanies(companyIds, trackChanges);
        if (companyIds.Count() != companies.Count()) throw new CollectionByIdsBadRequestException();

        var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companyDtos;
    }

    public CompanyDto GetCompany(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);

        if (company is null) throw new CompanyNotFoundException(companyId);

        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public CompanyDto Create(CompanyForCreationDto company)
    {
        var companyInstance = _mapper.Map<Company>(company);
        
        _repository.Company.CreateCompany(companyInstance);
        _repository.Save(); // Repository Manager Save Method

        var companyDtoInstance = _mapper.Map<CompanyDto>(companyInstance);
        return companyDtoInstance;
    }

    public (IEnumerable<CompanyDto> companyDtos, string companyIds) CreateCompanies(IEnumerable<CompanyForCreationDto> companies)
    {
        if (companies is null || !companies.Any()) throw new CompanyCollectionBadRequestException();

        var companyEntities = _mapper.Map<IEnumerable<Company>>(companies);
        foreach (var company in companyEntities)
        {
            _repository.Company.CreateCompany(company);
        }
        _repository.Save();

        var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var companyIds = string.Join(',', companyDtos.Select(c => c.Id));

        return (companyDtos, companyIds);
    }

    public void DeleteCompany(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);
        
        _repository.Company.DeleteCompany(company);
        _repository.Save();
    }
}
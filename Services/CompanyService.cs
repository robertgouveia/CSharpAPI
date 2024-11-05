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
    public async Task<IEnumerable<CompanyDto>> GetAllCompanies(bool trackChanges)
    {
        var companies = await _repository.Company.GetAllCompanies(trackChanges);
        
        // Data Transfer Object - Manual Mapping
        //var companiesDto = companies.Select(c => new CompanyDto(c.Id, c.Name ?? "", string.Join(' ', c.Address, c.Country))); 
            
        //Using an Auto Mapper - turning companies into IEnumerable<CompanyDto>
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
                
        return companiesDto;
        // We do not need a try catch block due to the exception handler picking up the error
    }

    public async Task<IEnumerable<CompanyDto>> GetCompanies(IEnumerable<Guid> companyIds, bool trackChanges)
    {
        if (companyIds is null) throw new IdParamsBadRequestException();

        var companies = await _repository.Company.GetCompanies(companyIds, trackChanges);
        if (companyIds.Count() != companies.Count()) throw new CollectionByIdsBadRequestException();

        var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companyDtos;
    }

    public async Task<CompanyDto> GetCompany(Guid companyId, bool trackChanges)
    {
        var companyInstance = await CheckCompanyAndIfExists(companyId, trackChanges);
        var companyDto = _mapper.Map<CompanyDto>(companyInstance);
        return companyDto;
    }

    public async Task<CompanyDto> Create(CompanyForCreationDto company)
    {
        var companyInstance = _mapper.Map<Company>(company);
        
        _repository.Company.CreateCompany(companyInstance);
        await _repository.Save(); // Repository Manager Save Method

        var companyDtoInstance = _mapper.Map<CompanyDto>(companyInstance);
        return companyDtoInstance;
    }

    public async Task<(IEnumerable<CompanyDto> companyDtos, string companyIds)> CreateCompanies(IEnumerable<CompanyForCreationDto> companies)
    {
        if (companies is null || !companies.Any()) throw new CompanyCollectionBadRequestException();

        var companyEntities = _mapper.Map<IEnumerable<Company>>(companies);
        foreach (var company in companyEntities)
        {
            _repository.Company.CreateCompany(company);
        }
        await _repository.Save();

        var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var companyIds = string.Join(',', companyDtos.Select(c => c.Id));

        return (companyDtos, companyIds);
    }

    public async Task DeleteCompany(Guid companyId, bool trackChanges)
    {
        var companyInstance = await CheckCompanyAndIfExists(companyId, trackChanges);
        _repository.Company.DeleteCompany(companyInstance);
        await _repository.Save();
    }

    public async Task UpdateCompany(Guid companyId, CompanyForUpdateDto company, bool trackChanges)
    {
        var companyInstance = await CheckCompanyAndIfExists(companyId, trackChanges);
        _mapper.Map(company, companyInstance);
        await _repository.Save();
    }

    private async Task<Company> CheckCompanyAndIfExists(Guid id, bool trackChanges)
    {
        var companyInstance = await _repository.Company.GetCompany(id, trackChanges);
        if (companyInstance is null) throw new CompanyNotFoundException(id);

        return companyInstance;
    }
}
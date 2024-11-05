using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Services;

internal sealed class EmployeeService : IEmployeeService
{
    // Holds access to the repository / logger
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployees(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        // Verification the company exists before fetching employees
        await CheckCompanyExists(companyId, trackChanges);
        
        // We now have the employees but also meta data for pagination
        var employeesWithMetaData = await _repository.Employee.GetEmployees(companyId, employeeParameters, trackChanges);
        var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);

        return (employeeDtos, employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeDto> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        await CheckCompanyExists(companyId, trackChanges);
        var employeeEntity = await CheckEmployeeExists(companyId, employeeId, trackChanges);

        var employeeDto = _mapper.Map<EmployeeDto>(employeeEntity);
        return employeeDto;
    }

    public async Task<EmployeeDto> CreateEmployee(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
    {
        await CheckCompanyExists(companyId, trackChanges);

        var employeeInstance = _mapper.Map<Employee>(employee);
        
        _repository.Employee.CreateEmployee(companyId, employeeInstance);
        await _repository.Save();

        var employeeDto = _mapper.Map<EmployeeDto>(employeeInstance);
        return employeeDto;
    }

    public async Task DeleteEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        await CheckCompanyExists(companyId, trackChanges);
        var employeeEntity = await CheckEmployeeExists(companyId, employeeId, trackChanges);
        
        _repository.Employee.DeleteEmployee(employeeEntity);
        await _repository.Save();
    }

    public async Task UpdateEmployee(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool compTrackChanges, bool empTrackChanges)
    {
        await CheckCompanyExists(companyId, compTrackChanges);
        var employeeEntity = await CheckEmployeeExists(companyId, employeeId, empTrackChanges);

        // Combines the two Left overwrites Right
        // This is only local and not DB unless changes are tracked
        _mapper.Map(employee, employeeEntity);
        await _repository.Save();
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatch(Guid companyId, Guid employeeId,
        bool compTrackChanges, bool empTrackChanges)
    {
        await CheckCompanyExists(companyId, compTrackChanges);
        var employeeEntity = await CheckEmployeeExists(companyId, employeeId, empTrackChanges);
        
        // Mapping for the creation of the Patch
        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        return (employeeToPatch, employeeEntity);
    }

    public async Task SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employee)
    {
        _mapper.Map(employeeToPatch, employee);
        await _repository.Save();
    }

    private async Task<Company> CheckCompanyExists(Guid id, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(id, trackChanges);
        if (company is null) throw new CompanyNotFoundException(id);
        return company;
    }

    private async Task<Employee> CheckEmployeeExists(Guid companyId, Guid id, bool trackChanges)
    {
        var employeeEntity = await _repository.Employee.GetEmployee(companyId, id, trackChanges);
        if (employeeEntity is null) throw new EmployeeNotFoundException(id);
        return employeeEntity;
    }
}
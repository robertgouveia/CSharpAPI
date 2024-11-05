using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

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

    public async Task<IEnumerable<EmployeeDto>> GetEmployees(Guid companyId, bool trackChanges)
    {
        // Verification the company exists before fetching employees
        var company = await _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);
        
        var employees = await _repository.Employee.GetEmployees(companyId, trackChanges);
        var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        
        return employeeDtos;
    }

    public async Task<EmployeeDto> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employee = await _repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
        if (employee is null) throw new EmployeeNotFoundException(employeeId);

        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public async Task<EmployeeDto> CreateEmployee(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeInstance = _mapper.Map<Employee>(employee);
        
        _repository.Employee.CreateEmployee(companyId, employeeInstance);
        await _repository.Save();

        var employeeDto = _mapper.Map<EmployeeDto>(employeeInstance);
        return employeeDto;
    }

    public async Task DeleteEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employee = await _repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
        if (employee is null) throw new EmployeeNotFoundException(employeeId);
        
        _repository.Employee.DeleteEmployee(employee);
        await _repository.Save();
    }

    public async Task UpdateEmployee(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool compTrackChanges, bool empTrackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, compTrackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeInstance = await _repository.Employee.GetEmployee(companyId, employeeId, empTrackChanges);
        if (employeeInstance is null) throw new EmployeeNotFoundException(employeeId);

        // Combines the two Left overwrites Right
        // This is only local and not DB unless changes are tracked
        _mapper.Map(employee, employeeInstance);
        await _repository.Save();
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatch(Guid companyId, Guid employeeId,
        bool compTrackChanges, bool empTrackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, compTrackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeEntity = await _repository.Employee.GetEmployee(companyId, employeeId, empTrackChanges);
        if (employeeEntity is null) throw new EmployeeNotFoundException(employeeId);
        
        // Mapping for the creation of the Patch
        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        return (employeeToPatch, employeeEntity);
    }

    public async Task SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employee)
    {
        _mapper.Map(employeeToPatch, employee);
        await _repository.Save();
    }
}
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

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        // Verification the company exists before fetching employees
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);
        
        var employees = _repository.Employee.GetEmployees(companyId, trackChanges);
        var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        
        return employeeDtos;
    }

    public EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employee = _repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
        if (employee is null) throw new EmployeeNotFoundException(employeeId);

        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public EmployeeDto CreateEmployee(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeInstance = _mapper.Map<Employee>(employee);
        
        _repository.Employee.CreateEmployee(companyId, employeeInstance);
        _repository.Save();

        var employeeDto = _mapper.Map<EmployeeDto>(employeeInstance);
        return employeeDto;
    }

    public void DeleteEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employee = _repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
        if (employee is null) throw new EmployeeNotFoundException(employeeId);
        
        _repository.Employee.DeleteEmployee(employee);
        _repository.Save();
    }

    public void UpdateEmployee(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool compTrackChanges, bool empTrackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, compTrackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeInstance = _repository.Employee.GetEmployee(companyId, employeeId, empTrackChanges);
        if (employeeInstance is null) throw new EmployeeNotFoundException(employeeId);

        // Combines the two Left overwrites Right
        // This is only local and not DB unless changes are tracked
        _mapper.Map(employee, employeeInstance);
        _repository.Save();
    }
}
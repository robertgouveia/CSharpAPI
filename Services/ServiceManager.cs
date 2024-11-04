using AutoMapper;
using Contracts;
using Service.Contracts;

namespace Services;

public class ServiceManager : IServiceManager
{
    // Lazy loaded
    // Only created when used
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly Lazy<ICompanyService> _companyService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
    {
        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, loggerManager, mapper));
        _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, loggerManager, mapper));
    }

    // The services are set to the lazy loaded value
    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
}
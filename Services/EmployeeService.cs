using Contracts;
using Service.Contracts;

namespace Services;

internal sealed class EmployeeService : IEmployeeService
{
    // Holds access to the repository / logger
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public EmployeeService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
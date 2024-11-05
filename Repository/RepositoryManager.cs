using Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext; // instance of the (tables)
    
    // Lazy Initialization
    // Means only created once used
    private readonly Lazy<ICompanyRepository> _companyRepository;
    private readonly Lazy<IEmployeeRepository> _employeeRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        
        // Creates a Lazy Instance of the repositories
        _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(repositoryContext));
        _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(repositoryContext));
    }

    public ICompanyRepository Company => _companyRepository.Value; // setting them as lazy
    public IEmployeeRepository Employee => _employeeRepository.Value;

    public async Task Save() => await _repositoryContext.SaveChangesAsync(); // saving the DB
}
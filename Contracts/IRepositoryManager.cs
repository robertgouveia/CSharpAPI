namespace Contracts;

// Used to instantiate and save changes
public interface IRepositoryManager
{
    ICompanyRepository Company { get; }
    IEmployeeRepository Employee { get; } // Reference to the tables
    Task Save(); // save implementation
}
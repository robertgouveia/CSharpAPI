namespace Service.Contracts;

// Holds instances of the services
public interface IServiceManager
{
    ICompanyService CompanyService { get; }
    IEmployeeService EmployeeService { get; }
}
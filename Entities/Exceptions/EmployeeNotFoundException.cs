namespace Entities.Exceptions;

public class EmployeeNotFoundException : NotFoundException
{
    public EmployeeNotFoundException(Guid employeeId) : base($"Employee with Id {employeeId} was not found in the specified company") { }
}
namespace Entities.Exceptions;

public class EmployeeNotFoundException(Guid employeeId)
    : NotFoundException($"Employee with Id {employeeId} was not found in the specified company");
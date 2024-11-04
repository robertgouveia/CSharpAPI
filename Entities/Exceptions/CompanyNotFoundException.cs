namespace Entities.Exceptions;

public class CompanyNotFoundException(Guid id) : NotFoundException($"The company with {id} does not exist");
namespace Entities.Exceptions;

public class CompanyNotFoundException : NotFoundException
{
    // Custom message input
    public CompanyNotFoundException(Guid id) : base($"The company with {id} does not exist") { }
}
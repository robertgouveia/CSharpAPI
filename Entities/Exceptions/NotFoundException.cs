namespace Entities.Exceptions;

// This is a wrapper
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
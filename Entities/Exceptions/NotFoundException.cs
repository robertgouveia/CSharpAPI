namespace Entities.Exceptions;

// This is a wrapper
public class NotFoundException(string message) : Exception(message);
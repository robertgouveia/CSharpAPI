namespace Entities.Exceptions;

public class IdParamsBadRequestException() : BadRequestException($"Parameters must hold an ID");
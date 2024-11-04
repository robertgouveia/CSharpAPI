using System.Net;
using Contracts;
using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CompanyEmployees;

// Allows for DI
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILoggerManager _logger;

    public GlobalExceptionHandler(ILoggerManager logger) => _logger = logger;
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Setting up the response
        httpContext.Response.ContentType = "application/json";

        // Contains the error
        var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

        if (contextFeature is not null)
        {
            httpContext.Response.StatusCode = contextFeature.Error switch
            {
                NotFoundException => StatusCodes.Status404NotFound, // Querying against what Exception
                _ => StatusCodes.Status500InternalServerError
            };
            
            // Log the contained error
            _logger.LogError($"Something went wrong: {contextFeature.Error}");
                    
            // Write the error response - Using our entity
            await httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = contextFeature.Error.Message // Using our Exception Entity Message
            }.ToString());
        }
        
        return true;
    }
}
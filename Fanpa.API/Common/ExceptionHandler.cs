using Microsoft.AspNetCore.Diagnostics;

namespace Fanpa.API.Common;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        await  httpContext.Response.WriteAsJsonAsync(new
        {
            error =  exception.Message
        }, cancellationToken);
        
        return true;
    }
}
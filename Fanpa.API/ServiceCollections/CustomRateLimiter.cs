using System.Threading.RateLimiting;

namespace Fanpa.API.ServiceCollections;

public static class CustomRateLimiter
{
    public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.User.Identity?.Name 
                                  ?? context.Connection.RemoteIpAddress?.ToString() 
                                  ?? "anonymous",
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        QueueLimit = 0,
                        PermitLimit = 1000,
                        Window = TimeSpan.FromMinutes(1)
                    });
            });

            options.OnRejected = async (context, token) =>
            {
                var response = context.HttpContext.Response;
                response.StatusCode = StatusCodes.Status429TooManyRequests;
                response.ContentType = "application/json";
                response.Headers["Retry-After"] = "60";

                await response.WriteAsJsonAsync("""{"error": "Too many requests!"}""", token);
            };
        });
        
        return services;
    }
}
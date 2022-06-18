using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OrderApplication.Middlewares;

namespace OrderApplication.Services
{
    public static class MiddlewareRunner
    {

        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }

        public static IApplicationBuilder RunCachingMiddleware(this IApplicationBuilder app)
        {
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();
            return app;
        }
        
        public static IApplicationBuilder RunHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health",new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            return app;
        }
        
    }
}
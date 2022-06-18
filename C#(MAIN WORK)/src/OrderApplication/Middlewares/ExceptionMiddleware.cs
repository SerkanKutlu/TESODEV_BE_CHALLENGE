using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OrderApplication.Exceptions;
using OrderApplication.Models;

namespace OrderApplication.Middlewares
{
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

            }
            catch (ProductNotFoundException ex)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
                _logger.LogInformation($"NOT FOUND EXCEPTION: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (OrderNotFoundException ex)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
                _logger.LogInformation($"NOT FOUND EXCEPTION: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (CustomerNotFoundException ex)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
                _logger.LogInformation($"NOT FOUND EXCEPTION: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (FormatException ex)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                _logger.LogInformation($"INVALID FORMAT EXCEPTION: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (ServerNotRespondingException ex)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.ServiceUnavailable;
                _logger.LogInformation($"REMOTE SERVER EXCEPTION: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (HttpRequestException ex)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.ServiceUnavailable;
                _logger.LogInformation($"REMOTE SERVER EXCEPTION: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                _logger.LogError($"UNEXPECTED EXCEPTION OCCURED: {ex.Message}");
                await HandleExceptionAsync(httpContext,ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
    }
}
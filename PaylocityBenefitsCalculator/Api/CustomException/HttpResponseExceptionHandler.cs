using Api.Models;
using System.Net;
using System.Runtime.Intrinsics.X86;

namespace Api.CustomException
{
    /// <summary>
    /// Create Custom Exception Filter to catch all custom exception
    /// </summary>
    public class HttpResponseCustomExceptionHandler
    {
        private readonly RequestDelegate _next;
        public HttpResponseCustomExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (AccessViolationException avEx)
            {
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch(EmployeeCustomException employeeEx)
            {
                await HandleExceptionAsync(httpContext, employeeEx);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the custom middleware.",
                Error = "Error from the custom middleware.",
            }.ToString());
        }
    }
}

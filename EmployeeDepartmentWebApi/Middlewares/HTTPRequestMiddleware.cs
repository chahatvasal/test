using Microsoft.AspNetCore.Http.HttpResults;

namespace EmployeeDepartmentWebApi.Middlewares
{
    public class HTTPRequestMiddleware
    {
        private readonly RequestDelegate _next;
        public HTTPRequestMiddleware(RequestDelegate next) { _next = next; }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("x-api-key"))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Missing or invalid API key.");
            }
        }

    }
}

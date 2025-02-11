using Microsoft.AspNetCore.Http.HttpResults;

namespace EmployeeDepartmentWebApi.Middlewares
{
    public class HTTPRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ExpectedApiKey = "cebrvbrubwifirufgeriuvgr";
        public HTTPRequestMiddleware(RequestDelegate next) { _next = next; }

       
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("x-api-key", out var apiKeyValue))
            {
                if(apiKeyValue.Equals(ExpectedApiKey))
                {
                    await _next(context);
                    return;
                }

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid API key.");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Missing API key.");
            }
        }

    }
}

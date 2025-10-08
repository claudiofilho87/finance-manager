using System.Net;
using System.Text.Json;
using FinanceManager.Shared;

namespace FinanceManager.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
            var response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var apiResponse = ApiResponse<string>.ErrorResponse("An unexpected error occurred.");
            var json = JsonSerializer.Serialize(apiResponse);

            await response.WriteAsync(json);
        }
    }
}
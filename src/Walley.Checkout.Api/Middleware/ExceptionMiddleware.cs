using System.Net;
using System.Text.Json;
using Walley.Checkout.Api.Models;

namespace Walley.Checkout.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, detail) = exception switch
        {
            OrderNotFoundException => ((int)HttpStatusCode.NotFound, "Resource Not Found", exception.Message),
            InvalidOrderIdException => ((int)HttpStatusCode.BadRequest, "Validation Error", exception.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "Internal Server Error", "An unexpected error occured.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new ApiErrorResponse
        {
            Message = message,
            StatusCode = statusCode,
            Detail = detail
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}

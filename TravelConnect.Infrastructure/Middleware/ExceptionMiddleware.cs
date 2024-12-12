using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using TravelConnect.Domain.Exceptions;
using System.Text.Json;


namespace TravelConnect.Infrastructure.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            logger.LogInformation("Handling request: {Name}", context.Request.Path);
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred: {Name}", ex.Message);
            logger.LogError(ex, "StackTrace: {Name}", ex.StackTrace);
            await GetResult(ex, context);
        }
        finally
        {
            logger.LogInformation("Finished handling request.");
        }
    }

    private static async Task GetResult(Exception exception, HttpContext context)
    {
        switch (exception)
        {
            case ValidationException validationException:
                await OnCustomValidationException(context, validationException);
                break;

            case NotFoundException notFoundException:
                await OnCustomNotFoundException(context, notFoundException);
                break;

            case UnauthorizedException unauthorizedAccessException:
                await OnCustomUnauthorizedException(context, unauthorizedAccessException);
                break;

            case BusinessRuleViolationException businessRuleViolationException:
                await OnCustomBusinessRuleViolationException(context, businessRuleViolationException);
                break;

            default:
                await SendResult(context, exception, HttpStatusCode.InternalServerError);
                break;
        }
    }

    private static async Task OnCustomBusinessRuleViolationException(HttpContext context, BusinessRuleViolationException exception)
    {
        await SendResult(context, exception, HttpStatusCode.UnprocessableContent);
    }

    private static async Task OnCustomValidationException(HttpContext context, ValidationException exception)
    {
        await SendResult(context, exception, HttpStatusCode.BadRequest);
    }

    private static async Task OnCustomNotFoundException(HttpContext context, NotFoundException exception)
    {
        await SendResult(context, exception, HttpStatusCode.NoContent);
    }

    private static async Task OnCustomUnauthorizedException(HttpContext context, UnauthorizedException exception)
    {
        await SendResult(context, exception, HttpStatusCode.Forbidden);
    }

    private static async Task SendResult(HttpContext context, Exception exception, HttpStatusCode code)
    {
        var message = GetMessage(exception);
        var response = new { message, success = false };
        var jsonResponse = JsonSerializer.Serialize(response);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(jsonResponse);
    }

    private static string GetMessage(Exception exception)
    {
        try
        {
            return exception.Message;
        }
        catch (Exception)
        {
            return "Not-Message-Defined";
        }
    }
}

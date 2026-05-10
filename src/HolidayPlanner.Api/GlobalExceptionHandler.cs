using FluentValidation;
using HolidayPlanner.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HolidayPlanner.Api;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private static readonly Action<ILogger, string, Exception> LogUnhandledException =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(0, "UnhandledException"), "Unhandled exception: {Message}");

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        LogUnhandledException(logger, exception.Message, exception);

        var (statusCode, title) = exception switch
        {
            ValidationException => (StatusCodes.Status422UnprocessableEntity, "Validation failed"),
            NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            DomainException => (StatusCodes.Status409Conflict, "A domain rule was violated"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = statusCode switch
            {
                StatusCodes.Status422UnprocessableEntity => "https://tools.ietf.org/html/rfc4918#section-11.2",
                StatusCodes.Status404NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                StatusCodes.Status409Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            },
            Detail = exception is ValidationException validationException
                ? string.Join("; ", validationException.Errors.Select(e => e.ErrorMessage))
                : null
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}

using Biblioteca.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Exceptions;

/// <summary>
/// Tratamento centralizado de exceções no padrão RFC 7807 (ProblemDetails).
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Erro não tratado na API. {TraceId} {RequestPath} {ExceptionType} {Message}",
            httpContext.TraceIdentifier,
            httpContext.Request.Path.ToString(),
            exception.GetType().Name,
            exception.Message);

        var (statusCode, title, detail) = MapException(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Type = $"https://httpstatuses.io/{statusCode}",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            options: null,
            contentType: "application/problem+json",
            cancellationToken: cancellationToken);

        return true;
    }

    private (int StatusCode, string Title, string Detail) MapException(Exception exception)
    {
        return exception switch
        {
            ArgumentException => (
                StatusCodes.Status400BadRequest,
                "Requisição inválida",
                exception.Message),

            ResourceNotFoundException => (
                StatusCodes.Status404NotFound,
                "Recurso não encontrado",
                exception.Message),

            KeyNotFoundException => (
                StatusCodes.Status404NotFound,
                "Recurso não encontrado",
                exception.Message),

            ConflictException => (
                StatusCodes.Status409Conflict,
                "Conflito",
                exception.Message),

            DomainException => (
                StatusCodes.Status400BadRequest,
                "Regra de negócio violada",
                exception.Message),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Erro interno do servidor",
                _environment.IsDevelopment()
                    ? exception.Message
                    : "Ocorreu um erro inesperado. Tente novamente mais tarde.")
        };
    }
}

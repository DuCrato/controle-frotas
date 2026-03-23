using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Infrastructure;

/// <summary>
/// Handler global de exceções que converte exceções em respostas HTTP padronizadas.
/// Implementa o padrão RFC 7807 (Problem Details for HTTP APIs).
/// </summary>
public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = httpContext.TraceIdentifier;

        logger.LogError(
            exception,
            "Erro não tratado. TraceId: {TraceId}, Tipo: {ExceptionType}, Mensagem: {Message}",
            traceId,
            exception.GetType().Name,
            exception.Message);

        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
            Status = GetStatusCode(exception),
            Title = GetTitle(exception),
            Detail = exception.Message,
        };

        // Adicionar TraceId para rastreamento
        problemDetails.Extensions["traceId"] = traceId;

        // Adicionar erros de validação se aplicável
        if (exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray());

            problemDetails.Extensions["errors"] = errors;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            ArgumentNullException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            InvalidOperationException => StatusCodes.Status409Conflict,
            NotImplementedException => StatusCodes.Status501NotImplemented,
            TimeoutException => StatusCodes.Status504GatewayTimeout,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(Exception exception) =>
        exception switch
        {
            ValidationException => "Erro de Validação",
            ArgumentException or ArgumentNullException => "Argumento Inválido",
            KeyNotFoundException => "Recurso Não Encontrado",
            InvalidOperationException => "Operação Inválida",
            NotImplementedException => "Funcionalidade Não Implementada",
            TimeoutException => "Tempo Limite Excedido",
            _ => "Erro Interno do Servidor"
        };
}
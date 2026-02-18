using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Infrastructure
{
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Status = exception switch
                {
                    ValidationException => StatusCodes.Status400BadRequest,
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    InvalidOperationException => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError
                },
                Title = exception switch
                {
                    ValidationException => "Erro de Validação",
                    KeyNotFoundException => "Recurso não encontrado",
                    InvalidOperationException => "Conflito de Regras",
                    _ => "Erro Interno"
                },
                Detail = exception.Message
            };

            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions["errors"] = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
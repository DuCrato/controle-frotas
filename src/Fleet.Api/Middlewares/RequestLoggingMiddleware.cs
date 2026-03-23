namespace Fleet.Api.Middlewares;

/// <summary>
/// Middleware para logging de requisições e respostas HTTP.
/// Útil para rastreamento de operações e debugging.
/// </summary>
public sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = context.TraceIdentifier;
        var request = context.Request;

        // Log da requisição
        logger.LogInformation(
            "Requisição iniciada. TraceId: {TraceId}, Método: {Method}, Caminho: {Path}, IP: {RemoteIP}",
            traceId,
            request.Method,
            request.Path,
            context.Connection.RemoteIpAddress);

        // Capturar a resposta original
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        var startTime = DateTime.UtcNow;

        try
        {
            await next(context);

            var duration = DateTime.UtcNow - startTime;

            // Log da resposta com sucesso
            logger.LogInformation(
                "Requisição concluída. TraceId: {TraceId}, StatusCode: {StatusCode}, Duração: {DurationMs}ms",
                traceId,
                context.Response.StatusCode,
                duration.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;

            logger.LogError(
                ex,
                "Requisição falhou. TraceId: {TraceId}, Duração: {DurationMs}ms",
                traceId,
                duration.TotalMilliseconds);

            throw;
        }
        finally
        {
            // Copiar resposta para stream original
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}

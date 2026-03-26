using System.Net;
using System.Text.Json;
using FluentValidation;
using THEONEEAD.Domain.Exceptions;

namespace THEONEEAD.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (ValidationException ex)
        {
            await WriteJsonAsync(context, (int)HttpStatusCode.BadRequest, new
            {
                titulo = "Validação",
                erros = ex.Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            });
        }
        catch (AcessoConteudoNegadoException ex)
        {
            await WriteJsonAsync(context, (int)HttpStatusCode.Forbidden, new { titulo = "Acesso negado", detalhe = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteJsonAsync(context, (int)HttpStatusCode.Unauthorized, new { titulo = "Não autorizado", detalhe = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operação inválida");
            await WriteJsonAsync(context, (int)HttpStatusCode.BadRequest, new { titulo = "Requisição inválida", detalhe = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado");
            await WriteJsonAsync(context, (int)HttpStatusCode.InternalServerError, new { titulo = "Erro interno" });
        }
    }

    private static async Task WriteJsonAsync(HttpContext context, int statusCode, object body)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(body));
    }
}

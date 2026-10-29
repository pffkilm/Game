using System.Text.Json;

namespace Gamification.Api.Middleware;

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Необработанное исключение: {Message}", ex.Message);
            
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            
            var response = new 
            { 
                status = 500, 
                title = "Internal Server Error",
                message = "Произошла ошибка на сервере." // Текст исключения не раскрываем
            };
            
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
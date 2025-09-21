using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json;

namespace ProjetoBMA.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, ProblemDetailsFactory problemDetailsFactory)
        {
            _next = next;
            _logger = logger;
            _problemDetailsFactory = problemDetailsFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "ArgumentException");
                var pd = _problemDetailsFactory.CreateProblemDetails(context, statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = pd.Status ?? StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(JsonSerializer.Serialize(pd));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                var pd = _problemDetailsFactory.CreateProblemDetails(context, statusCode: StatusCodes.Status500InternalServerError, detail: "Ocorreu um erro interno.");
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = pd.Status ?? StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(JsonSerializer.Serialize(pd));
            }
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}

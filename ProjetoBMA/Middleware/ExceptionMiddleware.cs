using Microsoft.AspNetCore.Mvc;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.ContentType = "application/problem+json";

                ProblemDetails pd;

                switch (ex)
                {
                    case ArgumentException argEx:
                        _logger.LogWarning(argEx, "ArgumentException");
                        pd = _problemDetailsFactory.CreateProblemDetails(
                            context,
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: argEx.Message
                        );
                        break;

                    case KeyNotFoundException _:
                        pd = _problemDetailsFactory.CreateProblemDetails(
                            context,
                            statusCode: StatusCodes.Status404NotFound,
                            detail: "Recurso não encontrado"
                        );
                        break;

                    case UnauthorizedAccessException _:
                        pd = _problemDetailsFactory.CreateProblemDetails(
                            context,
                            statusCode: StatusCodes.Status401Unauthorized,
                            detail: "Acesso não autorizado"
                        );
                        break;

                    default:
                        pd = _problemDetailsFactory.CreateProblemDetails(
                            context,
                            statusCode: StatusCodes.Status500InternalServerError,
                            detail: "Ocorreu um erro interno."
                        );
                        break;
                }

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

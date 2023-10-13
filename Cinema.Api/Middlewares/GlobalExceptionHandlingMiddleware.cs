using Cinema.Domain.CustomResponse;
using Cinema.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace CinemaApi.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) => _logger = logger;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next, IServiceProvider serviceProvider)
        {
            try
            {
                await next(context);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, ex, (int)HttpStatusCode.BadRequest, serviceProvider);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, (int)HttpStatusCode.NotFound, serviceProvider);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(context, ex, (int)HttpStatusCode.Unauthorized, serviceProvider);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, (int)HttpStatusCode.InternalServerError, serviceProvider);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex, int statusCode, IServiceProvider serviceProvider)
        {
            _logger.LogError(context.Request.Path, ex.Message, context.RequestAborted, ex);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var errorResponse = ServiceResponse<object>.ErrorResult(ex.Message);

            var jsonErrorResponse = JsonSerializer.Serialize(errorResponse);

            await context.Response.WriteAsync(jsonErrorResponse);
        }
    }
}

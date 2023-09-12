using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CinemaApi.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) => _logger = logger;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                var problemDetails = new ProblemDetails
                {
                    Title = "an error ocurred while proccessig your request",
                    Detail = ex.Message,
                    Status = (int)HttpStatusCode.InternalServerError
                };

                string json = JsonSerializer.Serialize(problemDetails);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
        }
    }
}

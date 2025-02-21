using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace WebApi.Middlware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode httpStatusCode)
        {
            _logger.LogError($"An error occurred: {exception.Message}");

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            var errorResponse = new
            {
                Message = exception.Message,
                StatusCode = (int)httpStatusCode,
                Details = exception.StackTrace
            };

            await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Domain.Exceptions;

namespace WebApi.Middlware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        private static readonly Dictionary<Type, HttpStatusCode> ExceptionStatusCodes = new()
        {
            { typeof(ValidationException), HttpStatusCode.BadRequest },
            { typeof(NotRegisteredException), HttpStatusCode.BadRequest },
            { typeof(ArgumentException), HttpStatusCode.BadRequest },
            { typeof(AuthenticationFailedException), HttpStatusCode.Unauthorized },
            { typeof(InvalidTokenException), HttpStatusCode.Unauthorized },
            { typeof(ForbiddenException), HttpStatusCode.Forbidden },
            { typeof(NotFoundException), HttpStatusCode.NotFound },
            { typeof(ConflictException), HttpStatusCode.Conflict },
            { typeof(LimitExceededException), HttpStatusCode.TooManyRequests }
        };

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
            catch (Exception ex)
            {
                var statusCode = ExceptionStatusCodes.TryGetValue(ex.GetType(), out var code)
                    ? code
                    : HttpStatusCode.InternalServerError;

                await HandleExceptionAsync(httpContext, ex, statusCode);
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
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
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest); // 400 Bad Request
            }
            catch (AuthenticationFailedException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Unauthorized); // 401 Unauthorized
            }
            catch (InvalidTokenException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Unauthorized); // 401 Unauthorized
            }
            catch (ForbiddenException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Forbidden); // 403 Forbidden
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.NotFound); // 404 Not Found
            }
            catch (NotRegisteredException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest); // 400 Bad Request
            }
            catch (ConflictException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Conflict); // 409 Conflict
            }
            catch (LimitExceededException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.TooManyRequests); // 429 Too Many Requests
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError); // 500 Internal Server Error
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
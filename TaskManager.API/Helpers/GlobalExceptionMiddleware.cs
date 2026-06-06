using System.Net;
using System.Text.Json;
using TaskManager.API.DTOs;
using TaskManager.API.Helpers;

namespace TaskManager.API.Helpers
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pass the request to the next middleware
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the full exception internally
                _logger.LogError(ex,
                    "Unhandled exception for request {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                // Return a clean JSON error to the client
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponseDto
            {
                TraceId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            };

            switch (exception)
            {
                case KeyNotFoundException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.NotFound;
                    response.StatusCode =
                        (int)HttpStatusCode.NotFound;
                    response.Message = exception.Message;
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.Unauthorized;
                    response.StatusCode =
                        (int)HttpStatusCode.Unauthorized;
                    response.Message = "Unauthorized access.";
                    break;

                case ArgumentException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.BadRequest;
                    response.StatusCode =
                        (int)HttpStatusCode.BadRequest;
                    response.Message = exception.Message;
                    break;
                case ConflictException conflict:
                    context.Response.StatusCode = conflict.StatusCode;
                    response.StatusCode = conflict.StatusCode;
                    response.Message = exception.Message;
                    break;

                default:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.InternalServerError;
                    response.StatusCode =
                        (int)HttpStatusCode.InternalServerError;
                    response.Message =
                        "An unexpected error occurred. Please try again later.";
                    break;
            }

            // Only show stack trace in Development — never in Production
            if (_environment.IsDevelopment())
            {
                response.Details = exception.StackTrace;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}
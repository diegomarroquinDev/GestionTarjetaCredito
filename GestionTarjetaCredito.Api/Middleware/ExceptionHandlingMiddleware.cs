using FluentValidation;
using GestionTarjetaCredito.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace GestionTarjetaCredito.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                BusinessException => HttpStatusCode.BadRequest,
                ValidationException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(
                    exception,
                    "Ocurrió un error no controlado.");
            }
            else
            {
                _logger.LogWarning(
                    exception,
                    "Ocurrió un error controlado.");
            }

            var response = new
            {
                status = (int)statusCode,
                message = GetMessage(exception),
                errors = GetValidationErrors(exception)
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }

        private static string GetMessage(Exception exception)
        {
            return exception switch
            {
                ValidationException => "Se encontraron errores de validación.",
                _ => exception.Message
            };
        }

        private static object? GetValidationErrors(Exception exception)
        {
            if (exception is not ValidationException validationException)
            {
                return null;
            }

            return validationException.Errors
                .Select(error => new
                {
                    property = error.PropertyName,
                    message = error.ErrorMessage
                })
                .ToList();
        }
    }
}
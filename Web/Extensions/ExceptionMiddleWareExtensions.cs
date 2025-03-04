using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Web.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var logger = context.RequestServices.GetRequiredService<ILogger<Exception>>();
                    logger.LogError($"An error occurred: {contextFeature.Error}");

                    context.Response.ContentType = "application/json";

                    var (statusCode, message) = contextFeature.Error switch
                    {
                        NotFoundException => (HttpStatusCode.NotFound, "The requested resource was not found."),
                        UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "You are not authorized to perform this action."),
                        _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
                    };

                    context.Response.StatusCode = (int)statusCode;

                    var response = new
                    {
                        StatusCode = (int)statusCode,
                        Message = message,
                        Error = contextFeature.Error.Message
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });
        });
    }
}
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace AppGuayaquil.Api.Middleware;

public static class ExceptionHandler
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionHandlerFeature != null)
                {
                    var ex = exceptionHandlerFeature.Error;
                    var code = HttpStatusCode.InternalServerError;
                    string message = "Se produjo un error interno del servidor.";
                    
                    if (ex is InvalidOperationException)
                    {
                        code = HttpStatusCode.NotFound;
                        message = ex.Message;
                    }
                    else if (ex is ArgumentException)
                    {
                        code = HttpStatusCode.BadRequest;
                        message = ex.Message;
                    }
                    else if (ex is UnauthorizedAccessException)
                    {
                        code = HttpStatusCode.Unauthorized;
                        message = ex.Message;
                    }

                    context.Response.StatusCode = (int)code;
                    context.Response.ContentType = "application/json";

                    var result = JsonSerializer.Serialize(new { error = message });
                    await context.Response.WriteAsync(result);
                }
            });
        });
    }
}

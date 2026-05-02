using System.Linq.Dynamic.Core.Exceptions;
using VendingApp.API.Response;

namespace VendingApp.API.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var message = "An internal server error occurred";
        int statusCode = StatusCodes.Status500InternalServerError;

        try
        {
            await next(context);
        }
        catch (System.Exception ex)
        {
            switch (ex)
            {
                case ArgumentException
                or ArgumentNullException
                or InvalidOperationException
                or ParseException:
                    message = ex.Message;
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case Exception:
                    message = $"{ex.GetType().FullName} {ex.Message}";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var model = Result<string>.Failure(message);
            await context.Response.WriteAsJsonAsync(model);
        }
    }
}

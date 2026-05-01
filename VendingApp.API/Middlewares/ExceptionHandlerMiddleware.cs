using VendingApp.API.Response;

namespace VendingApp.API.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var message = "";

        try
        {
            await next(context);
        }
        catch (System.Exception ex)
        {
            switch (ex)
            {
                case Exception:
                    message = ex.Message;
                    break;
            }

            context.Response.ContentType = "application/json";
            var model = Result<string>.Failure(message);
            await context.Response.WriteAsJsonAsync(model);
        }
    }
}

namespace Presentation.WebApi.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"[{DateTime.Now}] {context.Request.Method} {context.Request.Path} => [{context.Response.StatusCode}]");
        await _next(context);
    }
}

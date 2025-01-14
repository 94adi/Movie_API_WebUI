namespace Movie.API.Middleware;

public class RedirectSwaggerHomepageMiddleware
{
    private readonly RequestDelegate _next;

    public RedirectSwaggerHomepageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/swagger/index.html")
        {
            context.Response.Redirect("/index.html", permanent: false);
            return;
        }
        await _next(context);
    }
}

public static class RedirectSwaggerHomepageMiddlewareExtensions
{
    public static IApplicationBuilder UseRedirectSwaggerHomepage(
    this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RedirectSwaggerHomepageMiddleware>();
    }
}

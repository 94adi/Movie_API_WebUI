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
        var redirectFrom = new List<string> { "/swagger/index.html", "/swagger" };
        if (redirectFrom.Contains(context.Request.Path))
        {
            context.Response.Redirect("/index.html", permanent: true);
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

namespace Movie.WebUI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ExceptionHandlingMiddleware(RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment webHostEnvironment)
    {
        _next = next;
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");

            if (_webHostEnvironment.IsDevelopment())
            {
                httpContext.Response.Redirect("/home/error?message=" + ex.Message);
            }
            else
            {
                httpContext.Response.Redirect("/home/error");
            }
        }
    }
}

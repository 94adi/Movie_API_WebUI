using Movie.BuildingBlocks.Utils;

namespace Movie.WebUI.Extensions;

public static class DependencyInjectionExtensions
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder appBuilder)
    {
        var environment = EnvironmentUtils.GetEnvironmentVariable();

        var assembly = typeof(Program).Assembly;

        appBuilder.Services.AddControllersWithViews();

        appBuilder.Services.AddAutoMapper(typeof(MapperProfile));

        appBuilder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });

        appBuilder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opt =>
            {
                opt.Cookie.HttpOnly = false;
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                opt.SlidingExpiration = true;
                //opt.LoginPath = "";
                //opt.AccessDeniedPath = "";
            });

        appBuilder.Services.AddSession(opt =>
        {
            opt.IdleTimeout = TimeSpan.FromMinutes(30);
            opt.Cookie.HttpOnly = true;
            opt.Cookie.IsEssential = true;
        });

        appBuilder.Services.AddHttpClient();
        appBuilder.Services.AddSingleton<IApiMessageRequestBuilder, ApiMessageRequestBuilder>();
        appBuilder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        appBuilder.Services.AddScoped<IBaseHttpService, BaseHttpService>();
        appBuilder.Services.AddScoped<ITokenProvider, TokenProvider>();
        appBuilder.Services.AddScoped<IUserService, UserService>();
        appBuilder.Services.AddScoped<IMovieService, MovieService>();
        appBuilder.Services.AddScoped<IGenreService, GenreService>();
        appBuilder.Services.AddScoped<IReviewService, ReviewService>();

        return appBuilder;
    }
}

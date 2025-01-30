namespace Movie.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder appBuilder)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        string connectionString = appBuilder.GetDatabaseConnectionString(environment);

        appBuilder.Services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(15),
                    errorNumbersToAdd: null
                );
            });
        });

        appBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        appBuilder.Services.AddControllers();

        appBuilder.Services.AddEndpointsApiExplorer();

        appBuilder.Services.AddAutoMapper(typeof(MapperProfile));

        var assembly = typeof(Program).Assembly;

        appBuilder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        appBuilder.Services.AddValidatorsFromAssembly(assembly);

        appBuilder.Services.AddScoped<IMovieRepository, MovieRepository>();
        appBuilder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        appBuilder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        appBuilder.Services.AddScoped<ITokenService, TokenService>();
        appBuilder.Services.AddScoped<IUserRepository, UserRepository>();
        appBuilder.Services.AddScoped<ISeedDataService, SeedDataService>();
        appBuilder.Services.AddScoped<IDatabaseService, DatabaseService>();
        appBuilder.Services.AddScoped<IUserService, UserService>();
        appBuilder.Services.AddScoped<IMovieService, MovieService>();
        appBuilder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        appBuilder.Services.AddScoped<IReviewService, ReviewService>();
        appBuilder.Services.AddScoped<IGenreRepository, GenreRepository>();
        appBuilder.Services.AddScoped<IMovieGenreRepository, MovieGenreRepository>();
        appBuilder.Services.AddScoped<IMovieCarouselRepository, MovieCarouselRepository>();
        appBuilder.Services.AddScoped<IRatingRepository, RatingRepository>();
        appBuilder.Services.AddScoped<IRatingService, RatingService>();
        appBuilder.Services.AddSingleton<IFileShareService, FileShareService>();

        appBuilder.Services.AddSwaggerGen();

        var key = appBuilder.GetPrivateKey();

        appBuilder.Services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            //dev settings
            opt.TokenValidationParameters = new TokenValidationParameters
            {

                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            //opt.RequireHttpsMetadata = false;
            opt.SaveToken = true;
        });

        appBuilder.Services.AddApiVersioning(opt =>
        {
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(2, 0);
            opt.ReportApiVersions = true;
        })
        .AddApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
            opt.DefaultApiVersion = new ApiVersion(2, 0);
        });

        appBuilder.Services.AddExceptionHandler<CustomExceptionHandler>();

        appBuilder.Services.AddHealthChecks().AddSqlServer(connectionString);

        return appBuilder;
    }
}

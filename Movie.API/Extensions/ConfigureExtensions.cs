namespace Movie.API.Extensions;

public static class ConfigureExtensions
{

    public static WebApplicationBuilder RegisterConfigs(this WebApplicationBuilder appBuilder)
    {
        var environment = EnvironmentUtils.GetEnvironmentVariable();

        RegisterConfigsHelper(appBuilder, environment);

        return appBuilder;
    }

    public static string GetDatabaseConnectionString(this WebApplicationBuilder builder, string environment)
    {
        environment = environment.ToLower();
        return environment switch
        {
            EnvironmentConstants.DEVELOPMENT =>
                builder.Configuration.GetConnectionString("Database"),

            EnvironmentConstants.DOCKER =>
                Environment.GetEnvironmentVariable("ConnectionStrings__Database"),

            EnvironmentConstants.AZURE => builder.Configuration["AzureDatabase:ConnectionString"],

            _ => throw new InvalidOperationException("Unknown environment. Cannot retrieve database connection string")
        };
    }

    private static void RegisterConfigsHelper(WebApplicationBuilder appBuilder, string environment)
    {
        environment = environment.ToLower();

        switch (environment)
        {
            case EnvironmentConstants.DEVELOPMENT:
                RegisterLocalConfigs(appBuilder);
                break;
            case EnvironmentConstants.DOCKER:
                RegisterDockerConfigs(appBuilder);
                break;
            case EnvironmentConstants.AZURE:
                RegisterAzureConfigs(appBuilder);
                break;
            default:
                throw new InvalidOperationException("Unknown environment");
        }
    }

    private static void RegisterAzureConfigs(WebApplicationBuilder appBuilder) 
    {
        var keyVaultName = appBuilder.Configuration["AzureConfig:KeyVaultName"];

        var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net");

        var secretClient = new SecretClient(keyVaultUri, new DefaultAzureCredential());

        var connectionStringsecret = secretClient.GetSecret(SecretNames.Database);
        var fileShareConnectionStringSecret = secretClient.GetSecret(SecretNames.AzureFileShare_ConnectionString);
        var fileShareNameSecret = secretClient.GetSecret(SecretNames.AzureFileShare_ShareName);
        var appSecret = secretClient.GetSecret(SecretNames.AppSettings_Secret);
        var adminPassword = secretClient.GetSecret(SecretNames.AdminUserPassword);
        var userPassword = secretClient.GetSecret(SecretNames.RegularUserPassword);

        appBuilder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "AzureDatabase:ConnectionString", connectionStringsecret.Value.Value},
            { "AzureFileShare:ConnectionString", fileShareConnectionStringSecret.Value.Value },
            { "AzureFileShare:ShareName", fileShareNameSecret.Value.Value },
            { "AppSettings:Secret", appSecret.Value.Value },
            { "UserPasswordSecrets:Admin", adminPassword.Value.Value },
            { "UserPasswordSecrets:User", userPassword.Value.Value }
        });

        appBuilder.Services.Configure<FileShareConfig>(appBuilder.Configuration.GetSection("AzureFileShare"));

        appBuilder.Services.Configure<UserPasswordSecrets>(appBuilder.Configuration.GetSection("UserPasswordSecrets"));
    }

    private static void RegisterLocalConfigs(WebApplicationBuilder appBuilder)
    {
        appBuilder.Services.Configure<UserPasswordSecrets>(appBuilder.Configuration.GetSection("UserPasswordSecrets"));
        appBuilder.Services.Configure<FileLocalConfig>(appBuilder.Configuration.GetSection("FileLocalConfig"));

    }

    private static void RegisterDockerConfigs(WebApplicationBuilder appBuilder)
    {
        appBuilder.Services.Configure<UserPasswordSecrets>(appBuilder.Configuration.GetSection("UserPasswordSecrets"));
        appBuilder.Services.Configure<FileLocalConfig>(appBuilder.Configuration.GetSection("UserPasswordSecrets"));
    }

}

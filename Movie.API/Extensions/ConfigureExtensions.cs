namespace Movie.API.Extensions;

public static class ConfigureExtensions
{

    public static WebApplicationBuilder RegisterConfigs(this WebApplicationBuilder appBuilder)
    {
        var environment = Environment
                .GetEnvironmentVariable(EnvironmentConstants.ENVIRONMENT_STRING)
                .ToLower();

        RegisterConfigsHelper(appBuilder, environment);

        return appBuilder;
    }

    private static void RegisterConfigsHelper(WebApplicationBuilder appBuilder, string environment)
    {
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

    }

    private static void RegisterDockerConfigs(WebApplicationBuilder appBuilder)
    {

    }

}

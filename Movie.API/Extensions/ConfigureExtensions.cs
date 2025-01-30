using Microsoft.Extensions.DependencyInjection;

namespace Movie.API.Extensions;

public static class ConfigureExtensions
{
    public static WebApplicationBuilder RegisterAzureConfigs(this WebApplicationBuilder appBuilder)
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

        return appBuilder;
    }
}

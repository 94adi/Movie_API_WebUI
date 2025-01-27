using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Movie.API.Utils;

namespace Movie.WebUI.Extensions;

public static class ConfigureExtensions
{
    public static WebApplicationBuilder RegisterAzureConfigs(this WebApplicationBuilder appBuilder)
    {
        var keyVaultName = appBuilder.Configuration["AzureConfig:KeyVaultName"];

        var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net");

        var secretClient = new SecretClient(keyVaultUri, new DefaultAzureCredential());

        var movieApiBaseSecret = secretClient.GetSecret(SecretNames.MovieApiBase);

        var movieApiVersionSecret = secretClient.GetSecret(SecretNames.MovieApiVersion);

        appBuilder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "AzureApiConfig:MovieApiBase",  movieApiBaseSecret.Value.Value},
            { "AzureApiConfig:MovieApiVersion", movieApiVersionSecret.Value.Value }
        });

        return appBuilder;
    }
}

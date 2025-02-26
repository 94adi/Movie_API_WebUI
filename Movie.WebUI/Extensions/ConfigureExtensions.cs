using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Movie.API.Utils;
using Movie.BuildingBlocks.Utils;
using System;

namespace Movie.WebUI.Extensions;

public static class ConfigureExtensions
{
    public static void RegisterConfigs(this WebApplicationBuilder appBuilder)
    {
        var environment = EnvironmentUtils.GetEnvironmentVariable();
        switch (environment)
        {
            case EnvironmentConstants.DEVELOPMENT:
            case EnvironmentConstants.DOCKER:
                RegisterLocalConfigs(appBuilder);
            break;

            case EnvironmentConstants.AZURE:
                RegisterAzureConfigs(appBuilder);
            break;

            default:
                throw new InvalidOperationException("Unknown environment");

        }
    }

    public static void RegisterLocalConfigs(WebApplicationBuilder appBuilder)
    {
        appBuilder.Services.Configure<MovieApiConfig>(appBuilder.Configuration.GetSection("ApiConfig"));
        appBuilder.Services.Configure<MovieAppConfig>(appBuilder.Configuration.GetSection("AppConfig"));
    }


    public static void RegisterAzureConfigs(WebApplicationBuilder appBuilder)
    {
        var keyVaultName = appBuilder.Configuration["AzureConfig:KeyVaultName"];

        var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net");

        var secretClient = new SecretClient(keyVaultUri, new DefaultAzureCredential());

        var movieApiBaseSecret = secretClient.GetSecret(SecretNames.MovieApiBase);

        var movieApiVersionSecret = secretClient.GetSecret(SecretNames.MovieApiVersion);

        appBuilder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "ApiConfig:MovieApiBase",  movieApiBaseSecret.Value.Value},
            { "ApiConfig:MovieApiVersion", movieApiVersionSecret.Value.Value }
        });

        appBuilder.Services.Configure<MovieApiConfig>(appBuilder.Configuration.GetSection("ApiConfig"));
        appBuilder.Services.Configure<MovieAppConfig>(appBuilder.Configuration.GetSection("AppConfig"));
    }

    public static bool IsDockerEnv(this IHostEnvironment env)
    {
        bool result = false;
        if(env.EnvironmentName.ToLower() == EnvironmentConstants.DOCKER)
        {
            result = true;
        }

        return result; 
    }
}

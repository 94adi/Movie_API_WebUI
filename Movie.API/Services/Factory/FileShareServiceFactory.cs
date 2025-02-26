namespace Movie.API.Services.Factory;

public static class FileShareServiceFactory
{
    public static IFileShareService Create(IServiceProvider serviceProvider) 
    {
        var environment = Environment
        .GetEnvironmentVariable(EnvironmentConstants.ENVIRONMENT_STRING)
        .ToLower();

        return environment switch
        {
            EnvironmentConstants.DEVELOPMENT or EnvironmentConstants.DOCKER
                => serviceProvider.GetRequiredService<LocalFileShareService>(),

            EnvironmentConstants.AZURE
                => serviceProvider.GetRequiredService<AzureFileShareService>(),

            _ => throw new InvalidOperationException("Unknown environment")
        };
    }
}

using Movie.API.Services.Poster;

namespace Movie.API.Services.Factory;

public static class MoviePosterServiceFactory
{

    public static IMoviePosterService Create(IServiceProvider serviceProvider)
    {
        var environment = EnvironmentUtils.GetEnvironmentVariable();

        return environment switch
        {
            EnvironmentConstants.DEVELOPMENT or EnvironmentConstants.DOCKER
                => serviceProvider.GetRequiredService<LocalMoviePosterService>(),

            EnvironmentConstants.AZURE
                => serviceProvider.GetRequiredService<AzureMoviePosterService>(),

            _ => throw new InvalidOperationException("Unknown environment")
        };
    }
}
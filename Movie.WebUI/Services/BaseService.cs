namespace Movie.WebUI.Services;

public abstract class BaseService
{
    protected readonly string _baseApiUri;
    protected readonly MovieAppConfig _appConfig;
    protected readonly MovieApiConfig _apiConfig;

    protected BaseService(MovieApiConfig apiConfig,
        MovieAppConfig appConfig)
    {
        _apiConfig = apiConfig;
        _appConfig = appConfig;
        _baseApiUri = $"{_apiConfig.MovieApiBase}{_apiConfig.MovieApiVersion}";
    }

}

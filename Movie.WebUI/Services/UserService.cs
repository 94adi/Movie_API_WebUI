namespace Movie.WebUI.Services;

public class UserService : IUserService
{

    private readonly IBaseHttpService _httpService;
    private readonly MovieAppConfig _appConfig;
    private readonly IHttpClientFactory _httpClientFactory;
    private string _baseApiUri;

    public UserService(IHttpClientFactory httpClientFactory,
        IBaseHttpService httpService,
        IOptions<MovieAppConfig> appConfig
        )
    {
        _httpService = httpService;
        _appConfig = appConfig.Value;
        _httpClientFactory = httpClientFactory;
        _baseApiUri = $"{_appConfig.MovieApiBase}{_appConfig.MovieApiVersion}";
    }

    public async Task<T> LoginAsync<T>(LoginRequestDto loginRequest)
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.POST,
            Data = loginRequest,
            Url = $"{_baseApiUri}{_appConfig.LoginPath}"
        };

        var response = await _httpService.SendAsync<T>(request, isAuthenticated: false);

        return response;
    }

    public async Task<T> LogoutAsync<T>(TokenDTO token)
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.POST,
            Data = token,
            Url = $"{_baseApiUri}{_appConfig.LogoutPath}"
        };

        var response = await _httpService.SendAsync<T>(request, isAuthenticated: true);

        return response;
    }

    public async Task<T> RegisterAsync<T>(RegisterationRequestDto registerRequest)
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.POST,
            Data = registerRequest,
            Url = $"{_baseApiUri}{_appConfig.RegisterPath}"
        };

        var response = await _httpService.SendAsync<T>(request, isAuthenticated: false);

        return response;
    }
}

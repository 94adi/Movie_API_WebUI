namespace Movie.WebUI.Services;

public class UserService : BaseService, IUserService
{

    private readonly IBaseHttpService _httpService;

    public UserService(IBaseHttpService httpService,
        IOptions<MovieAppConfig> appConfig,
        IOptions<MovieApiConfig> apiConfig) : base(apiConfig.Value, appConfig.Value)
    {
        _httpService = httpService;
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

    public async Task<T> LogoutAsync<T>(LogoutRequestDto logoutRequest)
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.POST,
            Data = logoutRequest.Token,
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

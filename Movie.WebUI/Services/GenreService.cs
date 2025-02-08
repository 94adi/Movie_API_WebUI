namespace Movie.WebUI.Services;

public record GetGenresResult(IEnumerable<GenreDto> Genres);

public class GenreService : BaseService, IGenreService
{

    private readonly IBaseHttpService _baseHttpService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GenreService> _logger;

    public GenreService(IBaseHttpService baseHttpService,
        IHttpClientFactory httpClientFactory,
        IOptions<MovieAppConfig> movieAppConfig,
        IOptions<MovieApiConfig> apiConfig,
        ILogger<GenreService> logger) : base(apiConfig.Value, movieAppConfig.Value)
    {
        _baseHttpService = baseHttpService;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<GetGenresResult> GetGenres()
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{_appConfig.GetAllGenres}",
            ContentType = ContentType.Json
        };

        var response = await _baseHttpService.SendAsync<APIResponse>(request, 
            isAuthenticated: false);


        bool isresponseValid = (response != null &&
            response.IsSuccess &&
            response.Result != null);

        string resultString = Convert.ToString(response.Result);

        if (isresponseValid)
        {
            try
            {
                var conversionResult = JsonConvert
                    .DeserializeObject<GetGenresResult>(resultString);

                return conversionResult;
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing movie data.");
            }
        }

        return new GetGenresResult(new List<GenreDto> { });
    }
}
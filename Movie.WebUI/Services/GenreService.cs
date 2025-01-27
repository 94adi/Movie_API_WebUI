using Microsoft.Identity.Client;

namespace Movie.WebUI.Services;

public record GetGenresResult(IEnumerable<GenreDto> Genres);

public class GenreService : BaseService, IGenreService
{

    private readonly IBaseHttpService _baseHttpService;
    private readonly IHttpClientFactory _httpClientFactory;

    public GenreService(IBaseHttpService baseHttpService,
        IHttpClientFactory httpClientFactory,
        IOptions<MovieAppConfig> movieAppConfig,
        IOptions<MovieApiConfig> apiConfig) : base(apiConfig.Value, movieAppConfig.Value)
    {
        _baseHttpService = baseHttpService;
        _httpClientFactory = httpClientFactory;
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

        var response = await _baseHttpService.SendAsync<ApiResponse>(request, 
            isAuthenticated: false);


        bool isresponseValid = (response != null &&
            response.IsSuccess &&
            response.Result != null);

        string resultString = Convert.ToString(response.Result);

        if (isresponseValid)
        {
            var conversionResult = JsonConvert
                .DeserializeObject<GetGenresResult>(resultString);

            return conversionResult;
        }

        return new GetGenresResult(new List<GenreDto> { });
    }
}

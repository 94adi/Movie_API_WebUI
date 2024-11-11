namespace Movie.WebUI.Services;

public record GetGenresResult(IEnumerable<GenreDto> Genres);

public class GenreService : IGenreService
{

    private readonly IBaseHttpService _baseHttpService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly MovieAppConfig _movieAppConfig;
    private string _baseApiUri;

    public GenreService(IBaseHttpService baseHttpService,
        IHttpClientFactory httpClientFactory,
        IOptions<MovieAppConfig> movieAppConfig)
    {
        _baseHttpService = baseHttpService;
        _httpClientFactory = httpClientFactory;
        _movieAppConfig = movieAppConfig.Value;
        _baseApiUri = $"{_movieAppConfig.MovieApiBase}{_movieAppConfig.MovieApiVersion}";
    }

    public async Task<GetGenresResult> GetGenres()
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{_movieAppConfig.GetAllGenres}",
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

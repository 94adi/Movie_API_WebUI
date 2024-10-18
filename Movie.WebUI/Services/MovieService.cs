namespace Movie.WebUI.Services;

public class MovieService : IMovieService
{
    private readonly IBaseHttpService _httpService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly MovieAppConfig _appConfig;
    private string _baseApiUri;

    public MovieService(IBaseHttpService httpService, 
        IHttpClientFactory httpClientFactory, 
        IOptions<MovieAppConfig> appConfig)
    {
        _httpService = httpService;
        _httpClientFactory = httpClientFactory;
        _appConfig = appConfig.Value;
        _baseApiUri = $"{_appConfig.MovieApiBase}{_appConfig.MovieApiVersion}";
    }

    public async Task<CreateMovieResultDto> CreateMovie(CreateMovieDto movieDto)
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.POST,
            Data = movieDto,
            Url = $"{_baseApiUri}{_appConfig.CreateMoviePath}"
        };

        var response = await _httpService.SendAsync<ApiResponse>(request, 
            isAuthenticated: true);

        bool isMovieCreated = (response != null && 
            response.IsSuccess && 
            response.Result != null);

        string resultString = Convert.ToString(response.Result);

        if (isMovieCreated)
        {
            CreateMovieResultDto createdMovie = JsonConvert
                    .DeserializeObject<CreateMovieResultDto>(resultString);

            return createdMovie;
        }

        return new CreateMovieResultDto { Id = int.MinValue };
    }

    public async Task<GetAllMoviesResultDto> GetAllMovies()
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{_appConfig.GetAllMoviesPath}"
        };

        var response = await _httpService.SendAsync<ApiResponse>(request, 
            isAuthenticated: false);

        GetAllMoviesResultDto getAllMoviesResult = null;

        bool isListAvailable = (response != null) &&
            (response.IsSuccess == true) &&
            (response.Result != null);

        var stringResult = Convert.ToString(response.Result);


		if (isListAvailable)
        {
			getAllMoviesResult = JsonConvert.
                DeserializeObject<GetAllMoviesResultDto>(stringResult);
        }

        return getAllMoviesResult;
    }

	public async Task<GetMovieByIdResultDto> GetMovieById(int id)
	{
        var apiRequest = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{_appConfig.MovieApiPath}{id}"
		};

		var response = await _httpService.SendAsync<ApiResponse>(apiRequest,
	            isAuthenticated: true);

		GetMovieByIdResultDto getMovieByIdResultDto = null;

		bool isMovieAvailable = (response != null) &&
			(response.IsSuccess == true) &&
			(response.Result != null);

		var stringResult = Convert.ToString(response.Result);


		if (isMovieAvailable)
		{
			getMovieByIdResultDto = JsonConvert.
				DeserializeObject<GetMovieByIdResultDto>(stringResult);
		}

		return getMovieByIdResultDto;
	}

	public IEnumerable<MovieDto> GetMovies(int pageNumber, int pageSize)
    {
        //TO DO
        throw new NotImplementedException();
    }

    public async Task<UpdateMovieResultDto> UpdateMovie(MovieDto movieDto)
    {
        
        var apiRequest = new ApiRequest
        {
            ApiType = ApiType.PUT,
            Data = movieDto,
            Url = $"{_baseApiUri}{_appConfig.UpdateMoviePath}{movieDto.Id}"
        };

        var response = await _httpService.SendAsync<ApiResponse>(apiRequest,
        isAuthenticated: true);

        bool isMovieUpdated = (response != null) &&
            (response.IsSuccess == true);

        return new UpdateMovieResultDto(isMovieUpdated);
    }

    public async Task<DeleteMovieResultDto> DeleteMovie(int movieId)
    {
        var apiRequest = new ApiRequest
        {
            ApiType = ApiType.DELETE,
            Data = movieId,
            Url = $"{_baseApiUri}{_appConfig.DeleteMoviePath}{movieId}"
        };

        var result = await _httpService.SendAsync<ApiResponse>(apiRequest, 
            isAuthenticated: true);

        bool isSucess = (result != null) && (result.IsSuccess == true);

        return new DeleteMovieResultDto(isSucess);
    }
}

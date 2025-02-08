using Microsoft.AspNetCore.Http.HttpResults;
using Movie.WebUI.Services.Handlers.Movies.Queries;

namespace Movie.WebUI.Services;

public class MovieService : BaseService, IMovieService
{
    private readonly IBaseHttpService _httpService;
    private readonly ILogger<MovieService> _logger;
    public MovieService(IBaseHttpService httpService,
        IOptions<MovieAppConfig> appConfig,
        IOptions<MovieApiConfig> apiConfig,
        ILogger<MovieService> logger) : base(apiConfig.Value, appConfig.Value)
    {
        _httpService = httpService;
        _logger = logger;
    }

    public async Task<CreateMovieResultDto> CreateMovie(CreateMovieDto movieDto)
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.POST,
            Data = movieDto,
            Url = $"{_baseApiUri}{_appConfig.CreateMoviePath}",
            ContentType = ContentType.MultipartFormData
        };

        var response = await _httpService.SendAsync<APIResponse>(request, 
            isAuthenticated: true);

        bool isMovieCreated = (response != null && 
            response.IsSuccess && 
            response.Result != null);

        if (isMovieCreated)
        {
            string resultString = Convert.ToString(response.Result);

            try
            {
                CreateMovieResultDto createdMovie = JsonConvert
        .           DeserializeObject<CreateMovieResultDto>(resultString);
                createdMovie.IsSuccess = true;
                return createdMovie;
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing movie data.");
            }
        }

        return new CreateMovieResultDto { Id = int.MinValue, IsSuccess = false };
    }

    public async Task<GetAllMoviesResultDto> GetAllMovies()
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{_appConfig.GetAllMoviesPath}"
        };

        var response = await _httpService.SendAsync<APIResponse>(request, 
            isAuthenticated: false);

        GetAllMoviesResultDto getAllMoviesResult = 
            new GetAllMoviesResultDto 
            { 
                MovieDtos = new List<MovieDto>() 
            };

        bool isListAvailable = ((response != null) &&
            (response.IsSuccess == true) &&
            (response.Result != null));
    
		if (isListAvailable)
        {
            var stringResult = Convert.ToString(response.Result);

            try
            {
                getAllMoviesResult = JsonConvert.
                    DeserializeObject<GetAllMoviesResultDto>(stringResult);
            }
            catch(JsonException e)
            {
                _logger.LogError(e, "Error deserializing movie data.");
            }
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

		var response = await _httpService.SendAsync<APIResponse>(apiRequest,
	            isAuthenticated: true);

		bool isResponseValid = ((response != null) &&
			(response.IsSuccess == true) &&
			(response.Result != null));

        if (!isResponseValid)
        {
            throw new Exception($"Movie with id {id} could not be found");
        }

        var stringResult = Convert.ToString(response.Result);

        try
        {
            var getMovieByIdResultDto = JsonConvert.
                        DeserializeObject<GetMovieByIdResultDto>(stringResult);

            return getMovieByIdResultDto;
        }
        catch (JsonException e)
        {
            _logger.LogError(e, "Error deserializing movie data.");
            throw;
        }      
	}

	public async Task<GetMoviesPagingResultDto> GetMovies(int pageNumber = 1, int pageSize = 0)
    {
        var apiRequest = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{_appConfig.GetAllMoviesPath}?pageNumber={pageNumber}&pageSize={pageSize}",
            ContentType = ContentType.MultipartFormData
        };

        var response = await _httpService.SendAsync<APIResponse>(apiRequest,
        isAuthenticated: true);

        bool isResponseValid = ((response != null) &&
                            (response.IsSuccess == true) &&
                            (response.Result != null));

        GetMoviesPagingResultDto resultDto = 
            new GetMoviesPagingResultDto(new List<MovieDto> { });

        if (isResponseValid)
        {
            try
            {
                var stringResult = Convert.ToString(response.Result);
                resultDto = JsonConvert.DeserializeObject<GetMoviesPagingResultDto>(stringResult);
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing movie data.");
            }
        }

        return resultDto;
    }

    public async Task<UpdateMovieResultDto> UpdateMovie(UpdateMovieDto movieDto)
    {     
        var apiRequest = new ApiRequest
        {
            ApiType = ApiType.PUT,
            Data = movieDto,
            Url = $"{_baseApiUri}{_appConfig.UpdateMoviePath}{movieDto.Id}",
            ContentType = ContentType.MultipartFormData
        };

        var response = await _httpService.SendAsync<APIResponse>(apiRequest,
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

        var result = await _httpService.SendAsync<APIResponse>(apiRequest, 
            isAuthenticated: true);

        bool isSucess = (result != null) && (result.IsSuccess == true);

        return new DeleteMovieResultDto(isSucess);
    }

    public async Task<GetAllMoviesCarouselResultDto> GetAllMoviesCarousel()
    {
        var apiRequest = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{_appConfig.GetAllMoviesCarousel}",
            ContentType = ContentType.Json
        };

        var response = await _httpService.SendAsync<APIResponse>(apiRequest, isAuthenticated: false);

        bool isResponseValid = (response != null) &&
                    (response.IsSuccess == true) &&
                    (response.Result != null);

        var resultDto = new GetAllMoviesCarouselResultDto
        {
            MovieCarousels = new List<Models.Dto.MovieDto>()
        };

        if (isResponseValid)
        {
            try
            {
                var stringResult = Convert.ToString(response.Result);
                resultDto = JsonConvert.DeserializeObject<GetAllMoviesCarouselResultDto>(stringResult);
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing movie data.");
            }
        }

        return resultDto;
    }

    public async Task<UpdateMovieCarouselResultDto> UpdateMovieCarousel(UpdateMovieCarouselDto request)
    {
        var apiRequest = new ApiRequest
        {
            ApiType = ApiType.POST,
            Data = request,
            Url = $"{_baseApiUri}{_appConfig.UpdateMovieCarousel}",
            ContentType = ContentType.MultipartFormData
        };

        var result = await _httpService.SendAsync<APIResponse>(apiRequest, isAuthenticated: true);

        bool isSucess = (result != null) && (result.IsSuccess);

        return new UpdateMovieCarouselResultDto(isSucess);
    }

    public async Task<GetMoviesCountResultDto> GetMoviesCount()
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{_appConfig.GetMoviesCountPath}"
        };

        var response = await _httpService.SendAsync<APIResponse>(request, isAuthenticated: false);

        bool isResponseAvailable = (response != null) &&
            (response.IsSuccess == true) &&
            (response.Result != null);

        if (isResponseAvailable)
        {
            var stringResult = Convert.ToString(response.Result);
            var result = JsonConvert.DeserializeObject<GetMoviesCountResultDto>(stringResult);
            return result;
        }

        _logger.LogWarning("Failed to retrieve movies count. Response: {response}", response);

        return new GetMoviesCountResultDto(0);
    }

    public async Task<RateMovieResultDto> RateMovie(RateMovieDto rateMovieRequest)
    {
        var urlBuild = new StringBuilder(_appConfig.RateMoviePath);
        urlBuild = urlBuild.Replace(oldValue: "{movieId}",
                            newValue: Convert.ToString(rateMovieRequest.MovieId));
        urlBuild = urlBuild.Replace(oldValue: "{rating}",
                            newValue: Convert.ToString(rateMovieRequest.RatingValue));

        var url = urlBuild.ToString();

        var request = new ApiRequest
        {
            ApiType = ApiType.POST,
            Data = { },
            Url = $"{_baseApiUri}{url}"
        };

        var result = await _httpService.SendAsync<APIResponse>(request, isAuthenticated: true);

        bool isSucess = (result != null) && (result.IsSuccess == true);

        return new RateMovieResultDto(isSucess);
    }

    public async Task<GetMovieRatingResultDto> GetMovieRating(int movieId)
    {
        var url = _appConfig.GetMovieRatingPath.Replace(oldValue: "{movieId}", 
                                newValue: Convert.ToString(movieId));

        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{url}"
        };

        var response = await _httpService.SendAsync<APIResponse>(request, isAuthenticated: false);

        bool isResponseAvailable = (response != null) &&
                                    (response.IsSuccess == true) &&
                                    (response.Result != null);

        GetMovieRatingResultDto result = null;

        if (isResponseAvailable)
        {
            try
            {
                var stringResult = Convert.ToString(response.Result);
                result = JsonConvert.
                    DeserializeObject<GetMovieRatingResultDto>(stringResult);
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing movie data.");
            }
        }

        return result;
    }

    public async Task<GetMovieRatingsResultDto> GetMovieRatings(int movieId, 
        GetMovieRatingsRequestDto request)
    {
        var url = _appConfig.GetMovieRatingsPath.Replace(oldValue: "{movieId}",
                               newValue: Convert.ToString(movieId));

        var apiRequest = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = request,
            Url = $"{_baseApiUri}{url}"
        };


        var response = await _httpService.SendAsync<APIResponse>(apiRequest, isAuthenticated: false);

        bool isResponseAvailable = (response != null) &&
                                    (response.IsSuccess == true) &&
                                    (response.Result != null);

        GetMovieRatingsResultDto result = new GetMovieRatingsResultDto
        {
            Ratings = new List<RatingDto>()
        };

        if (isResponseAvailable)
        {
            try
            {
                var stringResult = Convert.ToString(response.Result);
                result = JsonConvert.
                    DeserializeObject<GetMovieRatingsResultDto>(stringResult);
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing movie data.");
            }
        }

        return result;
    }
}

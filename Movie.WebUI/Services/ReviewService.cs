namespace Movie.WebUI.Services;

public class ReviewService : BaseService, IReviewService
{
    private readonly IBaseHttpService _httpService;
    private ILogger<ReviewService> _logger;

    public ReviewService(IBaseHttpService httpService,
        IOptions<MovieAppConfig> movieAppConfig,
        IOptions<MovieApiConfig> apiConfig,
        ILogger<ReviewService> logger) : base(apiConfig.Value, movieAppConfig.Value)
    {
        _httpService = httpService;
        _logger = logger;
    }

    public async Task<CreateReviewResultDto> AddReview(UpsertReviewDto reviewDto)
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.POST,
            Data = reviewDto,
            Url = $"{_baseApiUri}{_appConfig.AddMovieReviewPath}",
            ContentType = ContentType.MultipartFormData
        };

        var response = await _httpService.SendAsync<APIResponse>(request, isAuthenticated: true);

        bool isReviewCreated = (response != null && response.IsSuccess && response.Result != null);

        if (!isReviewCreated)
        {
            throw new Exception("Review could not be created");
        }

        string resultString = Convert.ToString(response.Result);

        try
        {
            CreateReviewResultDto createdReview = JsonConvert
                .DeserializeObject<CreateReviewResultDto>(resultString);

            return createdReview;
        }
        catch (JsonException e)
        {
            _logger.LogError(e, "Error deserializing movie data.");
            throw;
        }
    }

    public async Task<GetAllMovieReviewsResultDto> GetMovieReviews(int movieId, 
                                                                    int pageNumber,
                                                                    int pageSize)
    {
        var url = _appConfig.GetMovieReviewsPath
                    .Replace(oldValue: "{movieId}", newValue: Convert.ToString(movieId));

        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{url}?pageNumber={pageNumber}&pageSize={pageSize}"
        };

        var response = await _httpService.SendAsync<APIResponse>(request, isAuthenticated: false);

        bool isListAvailable = (response != null) &&
            (response.IsSuccess == true) &&
            (response.Result != null);

        var stringResult = Convert.ToString(response.Result);

        if (isListAvailable)
        {
            try
            {
                var getAllMovieReviewsResult = JsonConvert.
                    DeserializeObject<GetAllMovieReviewsResultDto>(stringResult);

                return getAllMovieReviewsResult;
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing movie data.");
            }
        }

        return new GetAllMovieReviewsResultDto 
        {
            ReviewDtos = new List<ReviewDto>() 
        };
    }

    public async Task<GetMovieReviewsCountResultDto> GetMovieReviewsCount(int movieId)
    {
        var url = _appConfig.GetMovieReviewsCountPath.Replace(oldValue: "{movieId}", 
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

        var stringResult = Convert.ToString(response.Result);

        if (isResponseAvailable)
        {
            var result = JsonConvert.DeserializeObject<GetMovieReviewsCountResultDto>(stringResult);
            return result;
        }

        _logger.LogWarning("Failed to retrieve movie reviews count. Response: {response}", response);
        return new GetMovieReviewsCountResultDto
        {
            Count = 0
        };
    }

    public async Task<GetMovieRatingByUserResultDto> GetMovieRatingByUser(int movieId)
    {
        var url = _appConfig.GetMovieRatingByUserPath
                        .Replace(oldValue: "{movieId}", newValue: Convert.ToString(movieId));

        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{url}"
        };

        var response = await _httpService.SendAsync<APIResponse>(request, isAuthenticated: true);

        bool isResponseAvailable = (response != null) &&
            (response.IsSuccess == true) &&
            (response.Result != null);

        var stringResult = Convert.ToString(response.Result);

        if (isResponseAvailable)
        {
            var getMovieRatingByUserResult = JsonConvert.
                DeserializeObject<GetMovieRatingByUserResultDto>(stringResult);
            return getMovieRatingByUserResult;
        }

        return new GetMovieRatingByUserResultDto();

    }

    public async Task<GetReviewByIdResultDto> GetReviewById(int reviewId)
    {
        var url = _appConfig.GetReviewByIdPath
                .Replace(oldValue: "{id}", newValue: Convert.ToString(reviewId));

        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{url}"
        };

        var response = await _httpService.SendAsync<APIResponse>(request, isAuthenticated: true);

        bool isResponseAvailable = (response != null) &&
            (response.IsSuccess == true) &&
            (response.Result != null);

        if (!isResponseAvailable)
        {
            throw new Exception($"Could not get review with id {reviewId}");
        }

        var stringResult = Convert.ToString(response.Result);

        try
        {
            var getMovieRatingByUserResult = JsonConvert.
                DeserializeObject<GetReviewByIdResultDto>(stringResult);

            return getMovieRatingByUserResult;
        }
        catch (JsonException e)
        {
            _logger.LogError(e, "Error deserializing movie data.");
            throw;
        }
    }

    public async Task<GetUserMovieReviewResultDto> GetUserMovieReview(int movieId, string userId)
    {
        var url = _appConfig.GetUserMovieReviewPath
                    .Replace(oldValue: "{movieId}", newValue: Convert.ToString(movieId))
                    .Replace(oldValue: "{userId}", newValue: userId);


        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{url}"
        };

        var response = await _httpService.SendAsync<APIResponse>(request, isAuthenticated: true);

        bool isResponseAvailable = (response != null) &&
           (response.IsSuccess == true) &&
           (response.Result != null);

        var stringResult = Convert.ToString(response.Result);

        if (isResponseAvailable)
        {
            try
            {
                var getUserMovieReviewResult = JsonConvert.
                    DeserializeObject<GetUserMovieReviewResultDto>(stringResult);

                return getUserMovieReviewResult;
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing movie data.");
            }
        }

        return new GetUserMovieReviewResultDto();
    }

    public async Task<UpdateReviewResultDto> UpdateReview(UpdateReviewDto updateReviewDto)
    {
        var url = _appConfig.UpdateReviewPath;

        var request = new ApiRequest
        {
            ApiType = ApiType.PUT,
            Data = updateReviewDto,
            Url = $"{_baseApiUri}{url}"
        };

        var response = await _httpService.SendAsync<APIResponse>(request, isAuthenticated: true);

        bool isResponseAvailable = (response != null) &&
            (response.IsSuccess == true) &&
            (response.Result != null);

        if (!isResponseAvailable)
        {
            return new UpdateReviewResultDto(false);
        }

        return new UpdateReviewResultDto(response.IsSuccess);      
    }
}

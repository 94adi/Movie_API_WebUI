namespace Movie.WebUI.Services;

public class ReviewService : IReviewService
{
    private readonly IBaseHttpService _httpService;
    private readonly MovieAppConfig _movieAppConfig;
    private string _baseApiUri;

    public ReviewService(IBaseHttpService httpService,
        IOptions<MovieAppConfig> movieAppConfigOptions)
    {
        _httpService = httpService;
        _movieAppConfig = movieAppConfigOptions.Value;
        _baseApiUri = $"{_movieAppConfig.MovieApiBase}{_movieAppConfig.MovieApiVersion}";
    }

    public async Task<CreateReviewResultDto> AddReview(UpsertReviewDto reviewDto)
    {
        var request = new ApiRequest
        {
            ApiType = ApiType.POST,
            Data = reviewDto,
            Url = $"{_baseApiUri}{_movieAppConfig.AddMovieReviewPath}",
            ContentType = ContentType.MultipartFormData
        };

        var response = await _httpService.SendAsync<ApiResponse>(request, isAuthenticated: true);

        bool isReviewCreated = (response != null && response.IsSuccess && response.Result != null);

        string resultString = Convert.ToString(response.Result);

        if (isReviewCreated)
        {
            CreateReviewResultDto createdReview = JsonConvert
                .DeserializeObject<CreateReviewResultDto>(resultString);

            return createdReview;
        }

        return new CreateReviewResultDto { Id = int.MinValue };
    }

    public async Task<GetAllMovieReviewsResultDto> GetMovieReviews(int movieId, 
                                                                    int pageNumber,
                                                                    int pageSize)
    {
        var url = _movieAppConfig.GetMovieReviewsPath
                    .Replace(oldValue: "{movieId}", newValue: Convert.ToString(movieId));

        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{url}?pageNumber={pageNumber}&pageSize={pageSize}"
        };

        var response = await _httpService.SendAsync<ApiResponse>(request, isAuthenticated: false);

        bool isListAvailable = (response != null) &&
            (response.IsSuccess == true) &&
            (response.Result != null);

        var stringResult = Convert.ToString(response.Result);

        GetAllMovieReviewsResultDto getAllMovieReviewsResult = null;

        if (isListAvailable)
        {
            getAllMovieReviewsResult = JsonConvert.
                DeserializeObject<GetAllMovieReviewsResultDto>(stringResult);
        }

        return getAllMovieReviewsResult;
    }

    public async Task<GetMovieReviewsCountResultDto> GetMovieReviewsCount(int movieId)
    {
        var url = _movieAppConfig.GetMovieReviewsCountPath.Replace(oldValue: "{movieId}", 
            newValue: Convert.ToString(movieId));

        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{url}"
        };

        var response = await _httpService.SendAsync<ApiResponse>(request, isAuthenticated: false);

        bool isResponseAvailable = (response != null) &&
            (response.IsSuccess == true) &&
            (response.Result != null);

        var stringResult = Convert.ToString(response.Result);

        if (isResponseAvailable)
        {
            var result = JsonConvert.DeserializeObject<GetMovieReviewsCountResultDto>(stringResult);
            return result;
        }

        throw new Exception("Could not get result");
    }

    public async Task<GetMovieRatingByUserResultDto> GetMovieRatingByUser(int movieId)
    {
        var url = _movieAppConfig.GetMovieRatingByUserPath
                        .Replace(oldValue: "{movieId}", newValue: Convert.ToString(movieId));

        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{url}"
        };

        var response = await _httpService.SendAsync<ApiResponse>(request, isAuthenticated: true);

        bool isResponseAvailable = (response != null) &&
            (response.IsSuccess == true) &&
            (response.Result != null);

        var stringResult = Convert.ToString(response.Result);

        GetMovieRatingByUserResultDto getMovieRatingByUserResult = null;

        if (isResponseAvailable)
        {
            getMovieRatingByUserResult = JsonConvert.
                DeserializeObject<GetMovieRatingByUserResultDto>(stringResult);
        }

        return getMovieRatingByUserResult;

    }

    public async Task<GetReviewByIdResultDto> GetReviewById(int reviewId)
    {
        var url = _movieAppConfig.GetReviewByIdPath
                .Replace(oldValue: "{id}", newValue: Convert.ToString(reviewId));

        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{url}"
        };

        var response = await _httpService.SendAsync<ApiResponse>(request, isAuthenticated: true);

        bool isResponseAvailable = (response != null) &&
            (response.IsSuccess == true) &&
            (response.Result != null);

        var stringResult = Convert.ToString(response.Result);

        GetReviewByIdResultDto getMovieRatingByUserResult = null;

        if (isResponseAvailable)
        {
            getMovieRatingByUserResult = JsonConvert.
                DeserializeObject<GetReviewByIdResultDto>(stringResult);
        }

        return getMovieRatingByUserResult;
    }

    public async Task<GetUserMovieReviewResultDto> GetUserMovieReview(int movieId, string userId)
    {
        var url = _movieAppConfig.GetUserMovieReviewPath
                    .Replace(oldValue: "{movieId}", newValue: Convert.ToString(movieId))
                    .Replace(oldValue: "{userId}", newValue: userId);


        var request = new ApiRequest
        {
            ApiType = ApiType.GET,
            Data = { },
            Url = $"{_baseApiUri}{url}"
        };

        var response = await _httpService.SendAsync<ApiResponse>(request, isAuthenticated: true);

        bool isResponseAvailable = (response != null) &&
           (response.IsSuccess == true) &&
           (response.Result != null);

        var stringResult = Convert.ToString(response.Result);

        GetUserMovieReviewResultDto getUserMovieReviewResult = null;

        if (isResponseAvailable)
        {
            getUserMovieReviewResult = JsonConvert.
                DeserializeObject<GetUserMovieReviewResultDto>(stringResult);
        }

        return getUserMovieReviewResult;
    }

    public async Task<UpdateReviewResultDto> UpdateReview(UpdateReviewDto updateReviewDto)
    {
        var url = _movieAppConfig.UpdateReviewPath;

        var request = new ApiRequest
        {
            ApiType = ApiType.PUT,
            Data = updateReviewDto,
            Url = $"{_baseApiUri}{url}"
        };

        var response = await _httpService.SendAsync<ApiResponse>(request, isAuthenticated: true);

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

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

    public async Task<CreateReviewResultDto> AddReview(CreateReviewDto reviewDto)
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

    public async Task GetReviewById(int reviewId)
    {
    }
}

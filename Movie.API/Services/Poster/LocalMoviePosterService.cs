namespace Movie.API.Services.Poster;

public class LocalMoviePosterService : IMoviePosterService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IFileShareService _fileShareService;
    private readonly FileLocalConfig _fileLocalConfig;

    public LocalMoviePosterService(IMovieRepository movieRepository, 
        IFileShareService fileShareService,
        IOptions<FileLocalConfig> fileLocalConfigOptions)
    {
        _movieRepository = movieRepository;
        _fileShareService = fileShareService;
        _fileLocalConfig = fileLocalConfigOptions.Value;
    }

    public async Task StoreMoviePoster(Models.Movie movie, IFormFile poster)
    {
        if(poster == null)
        {
            var movieFromDb = await _movieRepository.GetByIdAsync(movie.Id);
            if (!String.IsNullOrEmpty(movieFromDb.ImageUrl))
            {
                movie.ImageUrl = movieFromDb.ImageUrl;
            }
            else
            {
                movie.ImageUrl = "https://placehold.co/600x400";
            }

            return;
        }

        var movieId = movie.Id;
        string randomString = Utilities.GenerateRandomString(10);
        string systemFileName = movieId + "_" + randomString + Path.GetExtension(poster.FileName);

        await _fileShareService.UploadFileAsync(systemFileName, poster.OpenReadStream());

        var posterUrl = _fileShareService.GenerateFileUrl(systemFileName, TimeSpan.FromDays(999));

        movie.ImageFileName = systemFileName;
        movie.ImageUrl = posterUrl;
    }
}

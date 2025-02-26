
namespace Movie.API.Services.Poster;

public class AzureMoviePosterService : IMoviePosterService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IFileShareService _fileShareService;

    public AzureMoviePosterService(IMovieRepository movieRepository,
        IOptions<IFileShareService> fileShareServiceOptions)
    {
        _movieRepository = movieRepository;
        _fileShareService = fileShareServiceOptions.Value;
    }

    public async Task StoreMoviePoster(Models.Movie movie, IFormFile poster)
    {
        if (poster != null && poster.Length > 0)
        {
            var movieId = movie.Id;
            string fileGuid = Guid.NewGuid().ToString();
            string fileName = $"{movieId}_{fileGuid}{Path.GetExtension(poster.FileName)}";

            await _fileShareService.UploadFileAsync(fileName, poster.OpenReadStream());

            movie.ImageFileName = fileName;
        }
        else
        {
            var movieFromDb = await _movieRepository.GetByIdAsync(movie.Id);
            if (!String.IsNullOrEmpty(movieFromDb.ImageFileName))
            {
                movie.ImageFileName = movieFromDb.ImageFileName;
            }
        }
    }
}

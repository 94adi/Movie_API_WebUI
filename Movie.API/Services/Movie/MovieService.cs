namespace Movie.API.Services.Movie;

public class MovieService : IMovieService
{
    private readonly StorageSettings _storageSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMovieRepository _movieRepository;

    public MovieService(IOptions<StorageSettings> storageOptions,
        IHttpContextAccessor httpContextAccessor,
        IMovieRepository movieRepository)
    {
        _storageSettings = storageOptions.Value;
        _httpContextAccessor = httpContextAccessor;
        _movieRepository = movieRepository;
    }

    public async Task StoreMoviePoster(Models.Movie movie, IFormFile poster)
    {
        if(poster != null)
        {
            var movieId = movie.Id;
            string randomString = Utilities.GenerateRandomString(10);
            string fileName = movieId + "_" + randomString + Path.GetExtension(poster.FileName);

            string filePath = _storageSettings.ImageStoragePath + fileName;

            var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            FileInfo file = new FileInfo(directoryLocation);

            if (file.Exists)
            {
                file.Delete();
            }

            using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
            {
                poster.CopyTo(fileStream);
            }
            var httpContext = _httpContextAccessor.HttpContext;
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}/{httpContext.Request.PathBase.Value}";
            string imageUrl = baseUrl + _storageSettings.ImageStorageFolder + fileName;
            string imageLocationPath = filePath;

            movie.ImageLocalPath = imageLocationPath;
            movie.ImageUrl = imageUrl ??= "https://placehold.co/600x400";
        }
        else
        {
            var movieFromDb = await _movieRepository.GetByIdAsync(movie.Id);
            if(!String.IsNullOrEmpty(movieFromDb.ImageLocalPath) &&
                (!String.IsNullOrEmpty(movieFromDb.ImageUrl)))
            {
                movie.ImageLocalPath = movieFromDb.ImageLocalPath;
                movie.ImageUrl = movieFromDb.ImageUrl;
            }
            else
            {
                movie.ImageUrl = "https://placehold.co/600x400";
            }
        }

    }
}

namespace Movie.API.Services.Movie;

public class MovieService : IMovieService
{
    private readonly StorageSettings _storageSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMovieRepository _movieRepository;
    private readonly IMovieGenreRepository _movieGenreRepository;

    public MovieService(IOptions<StorageSettings> storageOptions,
        IHttpContextAccessor httpContextAccessor,
        IMovieRepository movieRepository,
        IMovieGenreRepository movieGenreRepository)
    {
        _storageSettings = storageOptions.Value;
        _httpContextAccessor = httpContextAccessor;
        _movieRepository = movieRepository;
        _movieGenreRepository = movieGenreRepository;
    }

    public async Task<Models.Movie> GetByIdAsync(int id, bool includeGenres = false)
    {
        if (includeGenres)
        {
            return await _movieRepository.GetByIdWithGenreAsync(id);
        }

        return await _movieRepository.GetByIdAsync(id);

    }

    public async Task StoreMoviePoster(Models.Movie movie, IFormFile poster)
    {
        if(poster != null)
        {
            var movieId = movie.Id;
            string randomString = Utilities.GenerateRandomString(10);
            string fileName = movieId + "_" + randomString + Path.GetExtension(poster.FileName);

            string filePath = _storageSettings.ImageStoragePath + fileName;

            var imageLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            var folderLocation = Path.Combine(Directory.GetCurrentDirectory(), _storageSettings.ImageStoragePath);

            if (!Directory.Exists(folderLocation))
            {
                Directory.CreateDirectory(folderLocation);
            }

            FileInfo file = new FileInfo(imageLocation);

            if (file.Exists)
            {
                file.Delete();
            }
            using (var fileStream = new FileStream(imageLocation, FileMode.Create))
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

    public async Task RemoveMovieGenres(int movieId)
    {
        var movieGenresToDelete = await _movieGenreRepository
                .GetAllAsync(mg => mg.MovieId == movieId);

        if (movieGenresToDelete != null && movieGenresToDelete.Count > 0)
        {
            foreach (var movieGenre in movieGenresToDelete)
            {
                await _movieGenreRepository.RemoveAsync(movieGenre);
            }
        }
    }

    public async Task AddMovieGenres(int movieId, IEnumerable<int> genreIds)
    {
        foreach (var genreId in genreIds)
        {
            var movieGenre = new Models.MovieGenre
            {
                GenreId = genreId,
                MovieId = movieId
            };

            await _movieGenreRepository.CreateAsync(movieGenre);
        }
    }
}

using Movie.API.Services.File;

namespace Movie.API.Services.Movie;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMovieGenreRepository _movieGenreRepository;
    private readonly IFileShareService _fileShareService;

    public MovieService(IMovieRepository movieRepository,
        IMovieGenreRepository movieGenreRepository,
        IFileShareService fileShareService)
    {
        _movieRepository = movieRepository;
        _movieGenreRepository = movieGenreRepository;
        _fileShareService = fileShareService;
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
        if(poster != null && poster.Length > 0)
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
            if(!String.IsNullOrEmpty(movieFromDb.ImageFileName))
            {
                movie.ImageFileName = movieFromDb.ImageFileName;
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

    public async Task AddPosterUrls(IEnumerable<Models.Movie> movies)
    {
        if ((movies == null) || !movies.Any())
        {
            return;
        }

        foreach(var movie in movies)
        {
            //TO DO: store in cache and verify if exists before calling
            movie.ImageUrl = _fileShareService.GenerateFileUrl(movie.ImageFileName, 
                new TimeSpan(1, 0, 0));
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

    public async Task<int> GetMoviesCount()
    {
        var movies = await _movieRepository.GetAllAsync();
        return movies.Count();
    }

    public async Task<IList<Models.Movie>> GetMoviesWithGenres(int pageNumber = 1, int pageSize = 0)
    {
        return await _movieRepository.GetMoviesWithGenres(pageNumber, pageSize);
    }
}
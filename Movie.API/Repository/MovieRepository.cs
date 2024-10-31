using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Movie.API.Repository;

public class MovieRepository : Repository<Movie.API.Models.Movie>, IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IList<Models.Movie>> GetMoviesWithGenres(int pageNumber = 1, int pageSize = 0)
    {
        var moviesWithGenres = _context.Movies.Include(m => m.MovieGenres)
            .ThenInclude(mg => mg.Genre);

        if (pageSize > 0)
        {
            moviesWithGenres.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }

        return moviesWithGenres.ToList();
    }

    public async Task<Models.Movie> GetByIdAsync(int id)
    {
        return await GetAsync((m) => m.Id == id, tracked: false);
    }

    public async Task<Models.Movie> UpdateAsync(Models.Movie movie)
    {
        _context.Attach(movie);
        movie.LatestUpdateDate = DateTime.Now;
        _context.Entry(movie).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return movie;
    }
}

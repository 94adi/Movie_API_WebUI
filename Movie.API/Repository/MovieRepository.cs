namespace Movie.API.Repository;

public class MovieRepository : Repository<Movie.API.Models.Movie>, IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
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

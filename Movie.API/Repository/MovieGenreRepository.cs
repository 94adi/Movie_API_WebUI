namespace Movie.API.Repository;

public class MovieGenreRepository : Repository<MovieGenre>, IMovieGenreRepository
{
    private readonly ApplicationDbContext _context;

    public MovieGenreRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}

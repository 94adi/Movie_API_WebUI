namespace Movie.API.Repository;

public class MovieCarouselRepository : Repository<MovieCarousel>, IMovieCarouselRepository
{
    private readonly ApplicationDbContext _context;
    public MovieCarouselRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<MovieCarousel> UpdateAsync(MovieCarousel movieCarousel)
    {
        _context.Attach(movieCarousel);
        _context.Entry(movieCarousel).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return movieCarousel;
    }
}

namespace Movie.API.Repository;

public class GenreRepository : Repository<Genre>, IGenreRepository
{
    private readonly ApplicationDbContext _context;

    public GenreRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Genre> UpdateAsync(Genre genre)
    {
        _context.Attach(genre);
        _context.Entry(genre).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return genre;
    }
}

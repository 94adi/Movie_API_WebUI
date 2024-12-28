
using Movie.API.Models;

namespace Movie.API.Repository;

public class RatingRepository : Repository<Models.Rating>, IRatingRepository
{
    private readonly ApplicationDbContext _context;

    public RatingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(Rating rating)
    {
        _context.Attach(rating);
        _context.Entry(rating).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}

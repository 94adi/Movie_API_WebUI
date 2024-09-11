using Movie.API.Models;
using Movie.API.Repository.Abstractions;

namespace Movie.API.Repository
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<RefreshToken> _refreshTokens;

        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _refreshTokens = _context.Set<RefreshToken>();
        }

        public async Task<RefreshToken> UpdateAsync(RefreshToken token)
        {
            _context.Entry(token).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return token;
        }
    }
}

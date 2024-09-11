namespace Movie.API.Repository.Abstractions
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> UpdateAsync(RefreshToken token);
    }
}

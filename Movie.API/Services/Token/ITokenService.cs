namespace Movie.API.Services.Token;

internal interface ITokenService
{
    bool IsAccessTokenValid(string accessToken, string userId, string tokenId);

    Task InvalidateTokens(string userId, string tokenId);

    Task InvalidateToken(RefreshToken refreshToken);
}

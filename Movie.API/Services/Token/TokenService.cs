namespace Movie.API.Services.Token
{
    public class TokenService(IRefreshTokenRepository repository) : ITokenService
    {
        public async Task<RefreshToken> GetRefreshTokenData(string refreshToken)
        {
            var result = await repository.GetAsync(rt => rt.Refresh_Token == refreshToken);

            return result;
        }

        public async Task InvalidateToken(RefreshToken refreshToken)
        {
            refreshToken.IsValid = false;
            await repository.UpdateAsync(refreshToken);
        }

        public async Task InvalidateTokens(string userId, string tokenId)
        {
            var token = await repository.GetAsync(t => t.UserId == userId &&
                t.JwtTokenId == tokenId);

            token.IsValid = false;

            await repository.UpdateAsync(token);
        }

        public bool IsAccessTokenValid(string accessToken, string userId, string tokenId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var jwt = tokenHandler.ReadJwtToken(accessToken);

                var jwtId = jwt.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Jti).Value;

                var uid = jwt.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Sub).Value;

                var result = (userId == uid) && (tokenId == jwtId);

                return result;
            }
            catch
            {
                return false;
            }

        }
    }
}

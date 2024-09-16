namespace Movie.API.Services.Handlers.Users.Commands.Token;

public record RefreshAccessTokenCommand(Models.Token token) : ICommand<RefreshAccessTokenResult>;

public record RefreshAccessTokenResult(Models.Token token);

internal class RefreshAccessTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository,
    ITokenService tokenService,
    ISender sender,
    IUserRepository userRepository) :
    ICommandHandler<RefreshAccessTokenCommand, RefreshAccessTokenResult>
{
    public async Task<RefreshAccessTokenResult> Handle(RefreshAccessTokenCommand command, 
        CancellationToken cancellationToken)
    {
        var currentRefreshToken = await refreshTokenRepository.GetAsync(t => 
            t.Refresh_Token == command.token.RefreshToken);

        bool isRefreshTokenValid = await IsRefreshTokenValid(currentRefreshToken, command.token);

        if (!isRefreshTokenValid) 
        {
            return new RefreshAccessTokenResult(null);
        }

        var userId = currentRefreshToken.UserId;

        var jwtId = currentRefreshToken.JwtTokenId;

        var generateRefreshTokenCommand = new GenerateRefreshTokenCommand(userId, jwtId);

        var generateRefreshTokenResult = await sender.Send(generateRefreshTokenCommand);

        var newRefreshToken = generateRefreshTokenResult.RefreshToken;

        await tokenService.InvalidateToken(currentRefreshToken);

        var user = await userRepository.GetAsync(u => u.Id == userId);

        if(user == null)
        {
            return new RefreshAccessTokenResult(null);
        }

        var generateAccessTokenCommand = new GenerateAccessTokenCommand(user, newRefreshToken.JwtTokenId);

        var newAccessTokenResult = await sender.Send(generateAccessTokenCommand);

        var newAccessToken = newAccessTokenResult;

        var result = new RefreshAccessTokenResult(new Models.Token
        {
            AccessToken = newAccessToken.Token,
            RefreshToken = newRefreshToken.Refresh_Token
        });

        return result;
    }


    private async Task<bool> IsRefreshTokenValid(RefreshToken refreshToken, Models.Token tokenDto)
    {
        if (refreshToken == null)
        {
            return false;
        }

        var isTokenValid = tokenService.IsAccessTokenValid(tokenDto.AccessToken,
            refreshToken.UserId,
            refreshToken.JwtTokenId);

        if (!isTokenValid)
        {
            await tokenService.InvalidateToken(refreshToken);
            return false;
        }

        if (!refreshToken.IsValid)
        {
            await tokenService.InvalidateTokens(refreshToken.UserId,
                refreshToken.JwtTokenId);
            return false;
        }

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            await tokenService.InvalidateToken(refreshToken);
            return false;
        }

        return true;
    }
}

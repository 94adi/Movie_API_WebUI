namespace Movie.API.Services.Handlers.Users.Commands.Token;

public record RevokeTokenCommand(Movie.API.Models.Token Token) : ICommand<RevokeTokenResult>;

public record RevokeTokenResult(bool IsSuccess);

internal class RevokeTokenCommandHandler(ITokenService tokenService) : 
    ICommandHandler<RevokeTokenCommand, RevokeTokenResult>
{
    public async Task<RevokeTokenResult> Handle(RevokeTokenCommand command,
        CancellationToken cancellationToken)
    {
        var refreshTokenObj = await tokenService.GetRefreshTokenData(command.Token.RefreshToken);
        
        if(refreshTokenObj == null)
        {
            return new RevokeTokenResult(false);
        }

        bool isTokenValid = tokenService.IsAccessTokenValid(command.Token.AccessToken,
            refreshTokenObj.UserId, refreshTokenObj.JwtTokenId);

        if (!isTokenValid)
        {
            return new RevokeTokenResult(false);
        }

        await tokenService.InvalidateTokens(refreshTokenObj.UserId, refreshTokenObj.JwtTokenId);

        return new RevokeTokenResult(true);
    }
}

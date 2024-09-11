namespace Movie.API.Services.Handlers.Users.Commands.Token;

public record GenerateRefreshTokenCommand(string UserId, string TokenId) 
    : ICommand<GenerateRefreshTokenResult>;

public record GenerateRefreshTokenResult(RefreshToken RefreshToken);

internal class GenerateRefreshTokenCommandHandler(IRefreshTokenRepository repository) :
ICommandHandler<GenerateRefreshTokenCommand, GenerateRefreshTokenResult>
{
    public async Task<GenerateRefreshTokenResult> Handle(GenerateRefreshTokenCommand command, 
        CancellationToken cancellationToken)
    {
        var refreshToken = new RefreshToken
        {
            IsValid = true,
            UserId = command.UserId,
            JwtTokenId = command.TokenId,
            ExpiresAt = DateTime.UtcNow.AddMinutes(30),
            Refresh_Token = Guid.NewGuid + "-" + Guid.NewGuid()
        };

        await repository.CreateAsync(refreshToken);

        return new GenerateRefreshTokenResult(refreshToken);
    }
}
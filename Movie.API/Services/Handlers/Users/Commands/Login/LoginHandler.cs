using BuildingBlocks.Exceptions;
using Movie.API.Services.Handlers.Users.Commands.Token;

namespace Movie.API.Services.Handlers.Users.Commands.Login;

public record LoginCommand(string Username, string Password) : ICommand<LoginResult>;

public record LoginResult(Models.Token Token);

internal class LoginCommandHandler(UserManager<ApplicationUser> userManager,
    IUserRepository userRepository,
    ISender sender) 
    : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(u => u.UserName.ToLower() == command.Username.ToLower());

        bool isUserValid = await userManager.CheckPasswordAsync(user, command.Password);

        bool isLoginInvalid = (user == null || !isUserValid);

        if (isLoginInvalid)
        {
            throw new BadRequestException("Invalid login credentials");
        }

        var jwtId = $"JTI{Guid.NewGuid()}";
        var accessToken = await sender.Send(new GenerateAccessTokenCommand(user, jwtId));
        var refreshToken = await sender.Send(new GenerateRefreshTokenCommand(user.Id, jwtId));

        var result = new LoginResult(new Models.Token
        {
            AccessToken = accessToken.Token,
            RefreshToken = refreshToken.RefreshToken.Refresh_Token
        });

        return result;
    }
}

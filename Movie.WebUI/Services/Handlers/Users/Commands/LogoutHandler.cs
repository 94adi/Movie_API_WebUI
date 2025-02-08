namespace Movie.WebUI.Services.Handlers.Users.Commands;

public record LogoutCommand(TokenDTO Token) : ICommand<LogoutResult>;

public record LogoutResult(bool IsSuccess);

public class LogoutCommandHandler(IUserService userService,
    IMapper mapper) : ICommandHandler<LogoutCommand, LogoutResult>
{
    public async Task<LogoutResult> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var apiRequest = mapper.Map<LogoutRequestDto>(command);
        var apiResponse = await userService.LogoutAsync<APIResponse>(apiRequest);

        if (apiResponse != null && apiResponse.IsSuccess) 
        {
            return new LogoutResult(true);
        }

        return new LogoutResult(false);
    }
}

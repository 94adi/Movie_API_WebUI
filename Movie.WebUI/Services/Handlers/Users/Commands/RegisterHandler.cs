using FluentValidation;


namespace Movie.WebUI.Services.Handlers.Users.Commands;

public class RegisterCommand : ICommand<RegisterResult>
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Password {  get; set; }
    public string Role { get; set; }
}

public record RegisterResult(bool IsSuccess, List<string>? ErrorMessages);

public class RegisterHandler(IUserService authService,
    IMapper mapper) 
    : ICommandHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(command.Role))
        {
            command.Role = StaticDetails.userRolesDict[UserRoles.USER];
        }

        var apiRequest = mapper.Map<RegisterationRequestDto>(command);

        var result = await authService.RegisterAsync<APIResponse>(apiRequest);

        return new RegisterResult(result.IsSuccess, result.Errors);
    }
}

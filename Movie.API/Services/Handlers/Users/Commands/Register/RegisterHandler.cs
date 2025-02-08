namespace Movie.API.Services.Handlers.Users.Commands.Register;

public record RegisterCommand(string UserName,string Name,string Password,string Role) 
    : ICommand<RegisterResult>;

public record RegisterResult(string Id,
    string UserName,
    string Name);

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(m => m.UserName)
            .NotEmpty().WithMessage("Username field must not be empty")
            .MinimumLength(3).WithMessage("UserName is required");

        RuleFor(m => m.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(10).WithMessage("Password must be at least 10 characters long.")
            .Matches(@"\d").WithMessage("Password must contain at least one digit.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"^[a-zA-Z0-9]*$").WithMessage("Password must not contain special characters.");

    }
}

internal class RegisterCommandHandler(UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IMapper mapper,
    IUserService userService
    ) : ICommandHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterCommand command, 
        CancellationToken cancellationToken)
    {

        bool isUserUnique = await userService.IsUniqueUser(command.UserName);

        if (!isUserUnique) 
        {
            throw new BadRequestException("Username already exists");
        }

        ApplicationUser user = new()
        {
            UserName = command.UserName,
            Email = command.UserName,
            NormalizedEmail = command.UserName.ToUpper(),
            Name = command.Name,
            RegisteredOn = DateTime.Now
        };

        var result = await userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded) 
        {
            throw new Exception("Registration process failed");
        }

        if (!roleManager.RoleExistsAsync(command.Role).GetAwaiter().GetResult())
        {
            await userManager.AddToRoleAsync(user, StaticDetails.userRolesDict[UserRoles.USER]);
        }
        else
        {
            await userManager.AddToRoleAsync(user, command.Role);
        }

        var newUser = await userService.GetUserByEmail(command.UserName);
        var newUserResult = mapper.Map<RegisterResult>(newUser);

        return newUserResult;
    }
}

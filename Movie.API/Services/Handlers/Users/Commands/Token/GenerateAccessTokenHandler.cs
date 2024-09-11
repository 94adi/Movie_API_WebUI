namespace Movie.API.Services.Handlers.Users.Commands.Token;

public record GenerateAccessTokenCommand(ApplicationUser User, string JWTokenId) : ICommand<GenerateAccessTokenResult>;

public record GenerateAccessTokenResult(string Token);

internal class GenerateAccessTokenCommandHandler(UserManager<ApplicationUser> userManager,
    IOptions<AppSettings> config) :
    ICommandHandler<GenerateAccessTokenCommand, GenerateAccessTokenResult>
{
    public async Task<GenerateAccessTokenResult> Handle(GenerateAccessTokenCommand command, CancellationToken cancellationToken)
    {
        var roles = await userManager.GetRolesAsync(command.User);

        var tokenHanlder = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(config.Value.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, command.User.UserName.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                new Claim(JwtRegisteredClaimNames.Jti, command.JWTokenId),
                new Claim(JwtRegisteredClaimNames.Sub, command.User.Id)
            }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHanlder.CreateToken(tokenDescriptor);

        var tokenString = tokenHanlder.WriteToken(token);

        return new GenerateAccessTokenResult(tokenString);
    }
}

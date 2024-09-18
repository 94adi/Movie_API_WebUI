using AutoMapper;
using BuildingBlocks.Exceptions;
using Movie.BuildingBlocks.CQRS;
using static Movie.BuildingBlocks.CQRS.ICommandHandler;

namespace Movie.WebUI.Services.Handlers.Users.Commands;

public record LoginCommand(string Username, string Password) : ICommand<LoginResult>;

public record LoginResult(bool IsSuccess);

internal class LoginHandler(IUserService authService,
    ITokenProvider tokenProvider,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) 
    : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var apiRequest = mapper.Map<LoginRequestDto>(command);
            var apiResponse = await authService.LoginAsync<ApiResponse>(apiRequest);

            if (apiResponse != null && apiResponse.IsSuccess)
            {
                //deserialize result and create token obj
                var token = JsonConvert.DeserializeObject<TokenDTO>(
                    Convert.ToString(apiResponse.Result));

                //create jwt out of it
                var handler = new JsonWebTokenHandler();
                var jwt = handler.ReadJsonWebToken(token.AccessToken);
                //create claims principal via claims identity made up if claims
                var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name,
                    jwt.Claims.FirstOrDefault(c => c.Type == "unique_name").Value));

                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role,
                    jwt.Claims.FirstOrDefault(c => c.Type == "role").Value));

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                //for cookie sing in
                await httpContextAccessor.HttpContext?.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal);

                tokenProvider.SetToken(token);
                return new LoginResult(true);
            }
        }
        catch (Exception) 
        {
            throw;
        }

        return new LoginResult(false);
    }
}

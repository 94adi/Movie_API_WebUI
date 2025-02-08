namespace Movie.WebUI.Services.Handlers.Users.Commands;

public record LoginCommand(string Username, string Password) : ICommand<LoginResult>;

public record LoginResult(bool IsSuccess, string ErrorMessage);

internal class LoginHandler(IUserService authService,
    ITokenProvider tokenProvider,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor, 
    ILogger<LoginHandler> logger) 
    : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, 
        CancellationToken cancellationToken)
    {
        APIResponse? APIResponse;
        LoginRequestDto apiRequest;
        apiRequest = mapper.Map<LoginRequestDto>(command);
        APIResponse = await authService.LoginAsync<APIResponse>(apiRequest);

        try
        {
            if (APIResponse != null && APIResponse.IsSuccess)
            {
                //deserialize result and create token obj
                var token = JsonConvert.DeserializeObject<TokenDTO>(
                    Convert.ToString(APIResponse.Result));

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

                var userId = jwt.Claims.FirstOrDefault(c => c.Type == "sub").Value;

                tokenProvider.SetToken(token);
                tokenProvider.SetUserId(userId);

                return new LoginResult(true, string.Empty);
            }
        }
        catch (Exception e) 
        {
            logger.LogError(e, "Login error");
        }

        return new LoginResult(false, APIResponse?.Errors.FirstOrDefault());
    }
}

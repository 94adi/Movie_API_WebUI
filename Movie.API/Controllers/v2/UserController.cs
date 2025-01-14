namespace Movie.API.Controllers.v2;

[ApiController]
[Route("api/v{version:apiVersion}/Identity")]
[ApiVersion("2.0")]
[Authorize]
public class UserController(IMapper mapper,
    ISender sender) : Controller
{

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<ActionResult<APIResponse>> Register([FromBody] RegisterUserRequest request)
    {
        var command = mapper.Map<RegisterCommand>(request);

        var result = await sender.Send(command);

        var response = mapper.Map<RegisterUserResponse>(result);

        APIResponse apiResponse = new APIResponse
        {
            Result = response,
            StatusCode = System.Net.HttpStatusCode.OK,
            IsSuccess = true
        };

        return Ok(apiResponse);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequest request)
    {
        var command = mapper.Map<LoginCommand>(request);

        var result = await sender.Send(command);

        var response = mapper.Map<LoginResponse>(result.Token);

        var apiResponse = new APIResponse
        {
            Result = response,
            IsSuccess = true,
            StatusCode = System.Net.HttpStatusCode.OK
        };

        return Ok(apiResponse);
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<APIResponse>> GenerateNewToken([FromBody] RefreshTokenRequest request)
    {
        var command = mapper.Map<RefreshAccessTokenCommand>(request);

        var result = await sender.Send(command);

        var response = mapper.Map<RefreshTokenResponse>(result);

        var apiResponse = new APIResponse
        {
            Result = response,
            IsSuccess = true,
            StatusCode = System.Net.HttpStatusCode.OK
        };

        return Ok(apiResponse);
    }

    [HttpPost("Revoke")]
    public async Task<ActionResult<APIResponse>> RevokeToken([FromBody] RevokeTokenRequest request)
    {
        var command = mapper.Map<RevokeTokenCommand>(request);

        var result = await sender.Send(command);

        APIResponse apiResponse;

        if (result.IsSuccess)
        {
            apiResponse = new APIResponse
            {
                Result = result,
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
        else
        {
            apiResponse = new APIResponse
            {
                Result = result,
                IsSuccess = false,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
        return Ok(apiResponse);

    }
}

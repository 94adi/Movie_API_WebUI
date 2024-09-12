using Movie.API.Services.Handlers.Users.Commands.Login;

namespace Movie.API.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/Identity")]
[ApiVersion("1.0")]
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
}

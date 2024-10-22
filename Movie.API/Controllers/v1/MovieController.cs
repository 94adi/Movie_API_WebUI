using Movie.API.Models;
using Movie.API.Services.Handlers.Movies.Commands.DeleteMovie;

namespace Movie.API.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/MovieAPI")]
[ApiVersion("1.0")]
[Authorize]
public class MovieController : Controller
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public MovieController(ISender sender,
        IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetMovies([FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 0)
    {
        var request = new GetMoviesRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = _mapper.Map<GetMoviesQuery>(request);
        var result = await _sender.Send(query);
        var response = _mapper.Map<GetMoviesResponse>(result);

        var apiResponse = new APIResponse
        {
            Result = response,
            StatusCode = System.Net.HttpStatusCode.OK
        };

        var pagination = _mapper.Map<Pagination>(request);

        if (pagination.PageSize > 0) 
        {
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
        }
        
        return Ok(apiResponse);
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateMovie([FromForm] CreateMovieRequest request)
    {
        var command = _mapper.Map<CreateMovieCommand>(request);

        var result = await _sender.Send(command);

        var response = _mapper.Map<CreateMovieResponse>(result);

        var apiResponse = new APIResponse
        {
            Result = response,
            StatusCode = System.Net.HttpStatusCode.Created
        };

        return CreatedAtRoute("GetMovie", new { id = response.Id }, apiResponse);
    }

    [HttpGet("{id:int}", Name = "GetMovie")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetMovie(int id)
    {
        var query = new GetMovieQuery(id);

        var result = await _sender.Send(query);

        var response = _mapper.Map<GetMovieResponse>(result);

        var apiResponse = new APIResponse
        {
            Result = response,
            StatusCode = System.Net.HttpStatusCode.OK
        };

        return Ok(apiResponse);
    }

    [HttpPut("{id:int}", Name ="UpdateMovie")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> UpdateMovie(int id, 
        [FromForm] UpdateMovieRequest request)
    {
        APIResponse apiResponse = null;
        try
        {
            if (request == null || request.Id != id) 
            {
                return BadRequest();
            }

            var command = _mapper.Map<UpdateMovieCommand>(request);

            var result = await _sender.Send(command);

            if (result.IsSuccess)
            {
                apiResponse = new APIResponse
                {
                    Result = { },
                    IsSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.NoContent
                };

                return Ok(apiResponse);
            }
            else
            {
                apiResponse = new APIResponse
                {
                    Result = { },
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Errors = new List<string>() { result.ErrorMessage }
                };

                return apiResponse;
            }


        }
        catch(Exception ex)
        {
            apiResponse = new APIResponse
            {
                Result = { },
                IsSuccess = false,
                Errors = new List<string>()
                {
                    ex.ToString()
                }
            };
        }

        return apiResponse;
    }

    [HttpDelete("{id:int}", Name ="DeleteMovie")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> DeleteMovie(int id)
    {
        var command = new DeleteMovieCommand(id);

        var result = await _sender.Send(command);

        APIResponse apiResponse = null;

        if (result.IsSuccess)
        {
            apiResponse = new APIResponse
            {
                Result = { },
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.NoContent
            };

            return Ok(apiResponse);
        }

        apiResponse = new APIResponse
        {
            Result = { },
            IsSuccess = false,
            StatusCode = System.Net.HttpStatusCode.InternalServerError,
            Errors = new List<string>() { result.ErrorMessage }
        };

        return apiResponse;
    }
}

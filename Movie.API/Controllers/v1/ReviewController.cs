namespace Movie.API.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ReviewController : Controller
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ReviewController(ISender sender,
        IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("/api/v{version:apiVersion}/Movie/{movieId}/Review")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetReviewsByMovie(int movieId, 
        [FromQuery]int pageNumber = 1,
        [FromQuery]int pageSize = 0)
    {
        var query = new GetReviewsByMovieQuery(movieId, pageNumber, pageSize);

        var result = await _sender.Send(query);

        var apiResponse = new APIResponse
        {
            Result = result,
            StatusCode = System.Net.HttpStatusCode.OK
        };

        var pagination = new Pagination(pageNumber, pageSize);

        if (pagination.PageSize > 0)
        {
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
        }
        return Ok(apiResponse);
    }

    [HttpGet("/api/v{version:apiVersion}/User/{userId}/Review")]
    public async Task<ActionResult<APIResponse>> GetReviewsByUser(string userId,
        [FromQuery]int pageNumber = 1,
        [FromQuery]int pageSize = 0)
    {
        var query = new GetReviewsByUserQuery(userId, pageNumber, pageSize);

        var result = await _sender.Send(query);

        var apiResponse = new APIResponse
        {
            Result = result,
            StatusCode = System.Net.HttpStatusCode.OK
        };

        var pagination = new Pagination(pageNumber, pageSize);

        if (pagination.PageSize > 0)
        {
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
        }
        return Ok(apiResponse);
    }

    [HttpGet("{id:int}", Name = "GetReview")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetReview(int id)
    {
        var query = new GetReviewQuery(id);

        var review = await _sender.Send(query);

        var apiResponse = new APIResponse
        {
            Result = review,
            StatusCode = System.Net.HttpStatusCode.OK
        };

        return Ok(apiResponse);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateReview([FromForm] CreateReviewRequest request)
    {
        var command = _mapper.Map<CreateReviewCommand>(request);

        var result = await _sender.Send(command);

        var response = _mapper.Map<CreateReviewResponse>(result);

        var apiResponse = new APIResponse
        {
            Result = response,
            StatusCode = System.Net.HttpStatusCode.Created
        };

        return CreatedAtRoute("GetReview", new { id = response.Id }, apiResponse);
    }
}

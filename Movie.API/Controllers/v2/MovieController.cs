using Azure.Core;
using Movie.API.Models.Responses;
using Movie.API.Services.Handlers.Genres.Queries.GetGenresByMovieId;
using Movie.API.Services.Handlers.Movies.Commands.AddGenresToMovie;
using Movie.API.Services.Handlers.Movies.Commands.DeleteMovie;
using Movie.API.Services.Handlers.Movies.Commands.UpdateMovieCarousel;
using Movie.API.Services.Handlers.Movies.Queries.GetMovieRating;
using Movie.API.Services.Handlers.Movies.Queries.GetMoviesCarousel;
using Movie.API.Services.Handlers.Movies.Queries.GetMoviesCount;

namespace Movie.API.Controllers.v2;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        var createMovieCommand = _mapper.Map<CreateMovieCommand>(request);

        var createdMovieResult = await _sender.Send(createMovieCommand);

        var createdMovieResponse = _mapper.Map<CreateMovieResponse>(createdMovieResult);

        await AddGenresToMovie(createdMovieResponse.Id, request.SelectedGenres, false);

        var apiResponse = new APIResponse
        {
            Result = createdMovieResponse,
            StatusCode = System.Net.HttpStatusCode.Created
        };

        return CreatedAtRoute("GetMovie", new { id = createdMovieResponse.Id }, apiResponse);
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
    [HttpGet("/api/v{version:apiVersion}/[controller]/{id}/Genre")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetMovieGenres(int id)
    {
        var query = new GetGenresByMovieIdQuery(id);

        var result = await _sender.Send(query);

        var response = _mapper.Map<GetMovieGenresResponse>(result);

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

                await AddGenresToMovie(id, request.SelectedGenres, true);

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

    [HttpGet("/api/v{version:apiVersion}/[controller]/Carousel")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetMovieCarousels()
    {
        var movieCarosels = await _sender.Send(new GetMoviesCarouselQuery());

        var apiResponse = new APIResponse
        {
            Result = movieCarosels,
            IsSuccess = true,
            StatusCode = System.Net.HttpStatusCode.OK
        };

        return Ok(apiResponse);
    }

    [HttpPost("/api/v{version:apiVersion}/[controller]/Carousel")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> AddToCarousel([FromForm] UpdateMovieCarouselRequest request)
    {
        var movieIds = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<string>>(request.MovieIds);       

        if (movieIds == null || (movieIds.Count() > 3 || movieIds.Count() == 0))
        {
            return BadRequest("You can add at most 3 movies to the carousel");
        }

        APIResponse apiResponse = null;

        var command = new UpdateMovieCarouselCommand(movieIds);

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

        apiResponse = new APIResponse
        {
            Result = { },
            IsSuccess = false,
            StatusCode = System.Net.HttpStatusCode.InternalServerError,
            Errors = new List<string>() { }
        };

        return apiResponse;

    }

    [HttpGet("/api/v{version:apiVersion}/[controller]/Count")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetMoviesCount()
    {
        var moviesCount = await _sender.Send(new GetMoviesCountQuery());

        var apiResponse = new APIResponse
        {
            Result = moviesCount,
            IsSuccess = true,
            StatusCode = System.Net.HttpStatusCode.OK
        };

        return Ok(apiResponse);
    }

    [HttpGet("/api/v{version:apiVersion}/[controller]/{movieId}/Rating")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetMovieRating(int movieId)
    {
        var finalRating = await _sender.Send(new GetMovieRatingQuery(movieId));

        var apiResponse = new APIResponse
        {
            Result = finalRating,
            IsSuccess = true,
            StatusCode = System.Net.HttpStatusCode.OK
        };
        
        return Ok(apiResponse);
    }

    private async Task AddGenresToMovie(int movieId, string genres, bool isUpdate = false)
    {
        var genreList = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<string>>(genres);

        var genreIds = genreList.Select(g => int.Parse(g)).ToList();

        var addGenresToMovieCommand = new AddGenresToMovieCommand(genreIds,
            movieId, isUpdate);

        await _sender.Send(addGenresToMovieCommand);
    }
}

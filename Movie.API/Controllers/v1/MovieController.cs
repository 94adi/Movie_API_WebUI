﻿using Azure;
using Movie.API.Models.Requests;
using Movie.API.Services.Handlers.Movies.CreateMovie;
using Movie.API.Services.Handlers.Movies.GetMovie;
using Movie.API.Services.Handlers.Movies.GetMovies;

namespace Movie.API.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/MovieAPI")]
[ApiVersion("1.0")]
//[Authorize]
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetMovies([AsParameters] GetMoviesRequest request)
    {
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
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
        }
        
        return Ok(apiResponse);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateMovie([FromBody] CreateMovieRequest request)
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
}
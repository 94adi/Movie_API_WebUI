using AutoMapper;
using Movie.API.Models.Requests;
using Movie.API.Models.Responses;
using Movie.API.Services.Handlers.Movies.CreateMovie;
using Movie.API.Services.Handlers.Movies.GetMovie;
using Movie.API.Services.Handlers.Movies.GetMovies;

namespace Movie.API.Mapper;

public class MapperProfile : Profile
{

    public MapperProfile() 
    {
        CreateMap<GetMoviesRequest, GetMoviesQuery>();
        CreateMap<GetMoviesResult, GetMoviesResponse>();

        CreateMap<GetMoviesQuery, Pagination>();

        CreateMap<CreateMovieRequest, CreateMovieCommand>();
        CreateMap<CreateMovieResult, CreateMovieResponse>();

        CreateMap<GetMovieResult, GetMovieResponse>();
    }
}

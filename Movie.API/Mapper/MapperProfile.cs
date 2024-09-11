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

        CreateMap<ApplicationUser, RegisterResult>();

        CreateMap<RegisterUserRequest, RegisterCommand>();
        CreateMap<RegisterResult, RegisterUserResponse>();
    }
}

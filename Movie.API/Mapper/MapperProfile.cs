﻿using Movie.API.Services.Handlers.Users.Commands.Login;
using Movie.API.Services.Handlers.Users.Commands.Token;

namespace Movie.API.Mapper;

public class MapperProfile : Profile
{

    public MapperProfile() 
    {
        CreateMap<GetMoviesRequest, GetMoviesQuery>();
        CreateMap<GetMoviesResult, GetMoviesResponse>();

        CreateMap<GetMoviesQuery, Pagination>();
        CreateMap<GetMoviesRequest, Pagination>();

        CreateMap<CreateMovieRequest, CreateMovieCommand>();
        CreateMap<CreateMovieResult, CreateMovieResponse>();

        CreateMap<UpdateMovieCommand, Models.Movie>();
        CreateMap<UpdateMovieRequest, UpdateMovieCommand>();

        CreateMap<GetMovieResult, GetMovieResponse>();

        CreateMap<ApplicationUser, RegisterResult>();

        CreateMap<RegisterUserRequest, RegisterCommand>();
        CreateMap<RegisterResult, RegisterUserResponse>();

        CreateMap<LoginRequest, LoginCommand>();
        CreateMap<Token, LoginResponse>().ReverseMap();

        CreateMap<RefreshTokenRequest, RefreshAccessTokenCommand>();
        CreateMap<RefreshAccessTokenResult, RefreshTokenResponse>();

        CreateMap<RevokeTokenRequest, RevokeTokenCommand>();

        CreateMap<CreateMovieRequest, CreateMovieCommand>();
    }
}

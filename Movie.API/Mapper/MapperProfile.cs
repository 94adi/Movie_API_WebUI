﻿using Movie.API.Models.Dto;
using Movie.API.Models;
using Movie.API.Services.Handlers.Genres.Commands.CreateGenre;
using Movie.API.Services.Handlers.Genres.Commands.UpdateGenre;
using Movie.API.Services.Handlers.Genres.Queries.GetGenres;
using Movie.API.Services.Handlers.Genres.Queries.GetGenresByMovieId;
using Movie.API.Services.Handlers.Reviews.Commands.UpdateReview;

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

        CreateMap<CreateReviewCommand, Models.Review>();

        CreateMap<CreateReviewRequest, CreateReviewCommand>();
        CreateMap<CreateReviewResult,  CreateReviewResponse>();

        CreateMap<Models.Review, Models.Dto.ReviewDto>().ReverseMap();

        CreateMap<UpdateGenreCommand,  Models.Genre>();

        CreateMap<CreateGenreResult, CreateGenreResponse>();
        CreateMap<GetGenresResult, GetGenresResponse>();
        CreateMap<GetGenresQuery, Pagination>();

        CreateMap<Genre, GenreDto>();

        CreateMap<Models.Movie, Models.Dto.MovieDto>()
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
            src.MovieGenres.Select(mg => new GenreDto
            {
                Id = mg.Genre.Id,
                Name = mg.Genre.Name
            }).ToList()));

        CreateMap<GetGenresByMovieIdResult, GetMovieGenresResponse>();

        CreateMap<Rating, RatingDto>();

        CreateMap<UpdateReviewRequest, UpdateReviewCommand>();
    }
}

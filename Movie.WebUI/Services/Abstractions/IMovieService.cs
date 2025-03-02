﻿namespace Movie.WebUI.Services.Abstractions;

public interface IMovieService
{
    Task<GetMoviesPagingResultDto> GetMovies(int pageNumber = 1, int pageSize = 0);

	Task<GetAllMoviesResultDto> GetAllMovies();

    Task<CreateMovieResultDto> CreateMovie(CreateMovieDto movieDto);

    Task<GetMovieByIdResultDto> GetMovieById(int id);

    Task<UpdateMovieResultDto> UpdateMovie(UpdateMovieDto movieDto);

    Task<DeleteMovieResultDto> DeleteMovie(int movieId);

    Task<UpdateMovieCarouselResultDto> UpdateMovieCarousel(UpdateMovieCarouselDto request);

    Task<GetAllMoviesCarouselResultDto> GetAllMoviesCarousel();

    Task<GetMoviesCountResultDto> GetMoviesCount();

    Task<RateMovieResultDto> RateMovie(RateMovieDto rateMovieRequest);

    Task<GetMovieRatingResultDto> GetMovieRating(int movieId);

    Task<GetMovieRatingsResultDto> GetMovieRatings(int movieId, GetMovieRatingsRequestDto request);
}

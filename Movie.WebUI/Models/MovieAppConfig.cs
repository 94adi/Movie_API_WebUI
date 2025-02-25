﻿namespace Movie.WebUI.Models;

public class MovieAppConfig
{
    public string MovieApiBase { get; set; }
    public string LoginPath { get; set; }
    public string RegisterPath { get; set; }
    public string MovieApiVersion { get; set; }
    public string MovieApiPath { get; set; }
    public string RefreshTokenPath { get; set; }
    public string LogoutPath { get; set; }
    public string CreateMoviePath { get; set; }
    public string GetAllMoviesPath { get; set; }
    public string UpdateMoviePath { get; set; }
    public string DeleteMoviePath { get; set; }
    public string GetAllGenres {  get; set; }
    public string UpdateMovieCarousel {  get; set; }
    public string GetAllMoviesCarousel { get; set; }
    public string PageSize { get; set; }
    public string NumberOfColumns {  get; set; }
    public string AddMovieReviewPath { get; set; }
    public string GetMovieReviewsPath { get; set; }
    public string GetMovieReviewsCountPath { get; set; }
    public string GetMoviesCountPath { get; set; }
    public string RateMoviePath { get; set; }
    public string GetMovieRatingByUserPath { get; set; }
    public string GetMovieRatingPath { get; set; }
    public string GetMovieRatingsPath { get; set; }
    public string GetReviewByIdPath { get; set; }
    public string UpdateReviewPath { get; set; }
    public string GetUserMovieReviewPath { get; set; }
}

﻿global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Movie.API.Data;
global using Movie.API.Models;
global using Microsoft.Extensions.Configuration;
global using Microsoft.IdentityModel.Tokens;
global using Asp.Versioning;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using AutoMapper;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Movie.API.Models.Requests;
global using Movie.API.Models.Responses;
global using System.Text.Json;
global using Microsoft.Extensions.Options;
global using Movie.BuildingBlocks.CQRS;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using static Movie.BuildingBlocks.CQRS.ICommandHandler;
global using Movie.API.Repository.Abstractions;
global using Movie.API.Services.Token;
global using Movie.API.Services.Handlers.Movies.CreateMovie;
global using Movie.API.Services.Handlers.Movies.Queries.GetMovie;
global using Movie.API.Services.Handlers.Movies.Queries.GetMovies;
global using Movie.API.Services.Handlers.Users.Commands.Register;
global using Movie.API.Mapper;
global using Movie.API.Repository;
global using Movie.API.Services.Seed;
global using Movie.API.Services.User;
global using Movie.API.Utils;
global using Movie.BuildingBlocks.Exceptions.Handler;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using Movie.API.Services.Handlers.Users.Commands.Login;
global using Movie.API.Services.Handlers.Users.Commands.Token;
global using BuildingBlocks.Exceptions;
global using Movie.BuildingBlocks;
global using Movie.API.Services.Handlers.Movies.Commands.UpdateMovie;
global using FluentValidation;
global using HealthChecks.UI.Client;
global using Movie.API.Services.Movie;
global using Movie.API.Models.Enums;
global using Movie.API.Services.Review;
global using Movie.BuildingBlocks.Behaviors;
global using Movie.API.Services.Handlers.Reviews.Commands.CreateReview;
global using Movie.API.Services.Handlers.Reviews.Queries.GetReviewById;
global using Movie.API.Services.Handlers.Reviews.Queries.GetReviewsByMovie;
global using Movie.API.Services.Handlers.Reviews.Queries.GetReviewsByUserId;
global using Movie.API.Services.Handlers.Genres.Commands.CreateGenre;
global using Movie.API.Services.Handlers.Genres.Queries.GetGenre;
global using Movie.API.Services.Handlers.Genres.Queries.GetGenres;
global using Movie.API.Services.Rating;
global using Movie.API.Services.Handlers.Ratings.Commands.AddUpdateRating;
global using Movie.API.Services.Handlers.Ratings.Queries.GetMovieRatingByUserId;
global using Movie.API.Services.Handlers.Ratings.Queries.GetMovieRatingsByUserIds;
global using Movie.API.Services.Handlers.Reviews.Commands.UpdateReview;
global using Movie.API.Services.Handlers.Reviews.Queries.GetReviewsCountByMovie;
global using Movie.API.Services.Handlers.Reviews.Queries.GetUserMovieReview;
global using Asp.Versioning.ApiExplorer;
global using Movie.API.Middleware;
global using Azure.Identity;
global using Azure.Security.KeyVault.Secrets;
global using Movie.API.Services.File;
global using Movie.API.Extensions;
global using Movie.BuildingBlocks.Models;
global using Movie.API.Services.Factory;
global using Movie.API.Services.Poster;
global using Movie.BuildingBlocks.Utils;
using System.ComponentModel.DataAnnotations;
using System.Net;
using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Movie.BuildingBlocks.Models;
using Movie.BuildingBlocks.Utils;

namespace Movie.BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(
    ILogger<CustomExceptionHandler> logger,
    ProblemDetailsFactory problemDetailsFactory)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An error occurred at {time}", DateTime.UtcNow);

        var details = exception switch
        {
            InternalServerException e =>
                (e.Message, "Internal Server Error", StatusCodes.Status500InternalServerError, null),

            FluentValidation.ValidationException e =>
                (e.Message, "Validation Error", 
                StatusCodes.Status400BadRequest,
                 e.Errors.Select(err =>  err.ErrorMessage).ToList()),

            BadRequestException e =>
                (e.Message, "Bad Request", StatusCodes.Status400BadRequest, null),

            NotFoundException e =>
                (e.Message, "Not Found", StatusCodes.Status404NotFound, null),

            _ =>
                (exception.Message, "Unhandled Exception", StatusCodes.Status500InternalServerError, null)
        };

        context.Response.StatusCode = details.Item3;

        var errors = details.Item4 == null ? 
            new List<string> { details.Item1 } : details.Item4;

        var response = new APIResponse
        {
            IsSuccess = false,
            Errors = errors,
            Result = details.Item2,
            StatusCode = StatusCodeConverter.ConvertToHttpStatusCode(details.Item3)
        };

        context.Response.StatusCode = details.Item3;
        await context.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
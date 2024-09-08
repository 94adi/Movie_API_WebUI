namespace Movie.API.Models.Requests;

public record GetMoviesRequest(int PageNumber = 1, int PageSize = 0){ }
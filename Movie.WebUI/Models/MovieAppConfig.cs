namespace Movie.WebUI.Models;

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
}

namespace Movie.BuildingBlocks.Exceptions;

public class AuthenticationException : Exception
{
    public AuthenticationException() : base("Authentication failed")
    {
        
    }
}

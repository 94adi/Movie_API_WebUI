namespace Movie.BuildingBlocks.Utils;

public static class EnvironmentUtils
{
    public static string GetEnvironmentVariable()
    {
        var environment = Environment.GetEnvironmentVariable(EnvironmentConstants.ENVIRONMENT_STRING)
            ?? EnvironmentConstants.DEVELOPMENT;

        return environment.ToLower();
    }

    public static string CapitalizeWords(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return string.Join(" ", input.Split(' ')
            .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
    }
}



public static class EnvironmentConstants
{
    public const string DEVELOPMENT = "development";
    public const string DOCKER = "docker";
    public const string AZURE = "azure";
    public const string ENVIRONMENT_STRING = "ASPNETCORE_ENVIRONMENT";
}

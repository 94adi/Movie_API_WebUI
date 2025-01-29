namespace Movie.API.Utils;

public static class Utilities
{
    public static byte[] GetPrivateKey(this WebApplicationBuilder builder)
    {
        var appSettingsSection = builder.Configuration.GetSection("AppSettings");
        builder.Services.Configure<AppSettings>(appSettingsSection);

        var appSettings = appSettingsSection.Get<AppSettings>();
        var key = Encoding.ASCII.GetBytes(appSettings.Secret);

        return key;
    }

    public static string GenerateRandomString(int length = 10)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static string GetAppUrl()
    {
        var urlsEnv = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
        string url = "";

        if (urlsEnv.Contains(";"))
        {
            var split = urlsEnv.Split(';');
            url = split[0];
        }
        else
        {
            url = urlsEnv;
        }

        return url;
    }

    public static string GetDatabaseConnectionString(this WebApplicationBuilder builder, string environment)
    {
        environment = environment.ToLower();
        return environment switch
        {
            "development" => builder.Configuration.GetConnectionString("Database"),
            "azure" => builder.Configuration["AzureDatabase:ConnectionString"],
            _ => builder.Configuration.GetConnectionString("Database")
        };
    }
}

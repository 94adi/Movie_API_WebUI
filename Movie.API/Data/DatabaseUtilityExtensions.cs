namespace Movie.API.Data;

public static class DatabaseUtilityExtensions
{
    public static async Task ApplyMigration(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                await _db.Database.MigrateAsync();
            }
        }
    }

    public static async Task SeedDatabase(this WebApplication app) 
    { 
        using var scope = app.Services.CreateScope();

        var serviceSeed = scope.ServiceProvider.GetRequiredService<ISeedDataService>();

        await serviceSeed.SeedAsync();
    }

    public static void DeleteDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var serviceSeed = scope.ServiceProvider.GetRequiredService<IDatabaseService>();

         serviceSeed.DropAllTables();
    }
}

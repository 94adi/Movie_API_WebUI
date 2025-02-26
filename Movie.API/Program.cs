var builder = WebApplication.CreateBuilder(args);

var environment = EnvironmentUtils.GetEnvironmentVariable();
//builder.Environment.EnvironmentName = environment;

builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Configuration.AddUserSecrets<Program>();

builder.RegisterConfigs();

builder.RegisterServices();

var app = builder.Build();

var isConverted = bool.TryParse(builder.Configuration["Settings:DeleteDatabaseOnBuild"], 
    out bool isDeleteDatabase);

if (isConverted && isDeleteDatabase)
{
    app.DeleteDatabase();
}

app.ApplyMigration().GetAwaiter().GetResult();
app.SeedDatabase().GetAwaiter().GetResult();

app.UseHttpsRedirection();

app.UseRouting();

app.UseSwagger();

app.UseCustomSwaggerUI();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler(opt => { });

app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).UseAuthorization();

app.UseRedirectSwaggerHomepage();

app.Run();
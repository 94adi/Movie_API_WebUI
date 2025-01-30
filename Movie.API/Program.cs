var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.RegisterAzureConfigs();

builder.RegisterServices();

var app = builder.Build();

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
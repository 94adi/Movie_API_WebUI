using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Movie.API.Data.ModelBuilders;

namespace Movie.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): 
        base(options)
    {
        
    }

    public DbSet<Models.Movie> Movies { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        MovieModelBuilder.BuildModel(modelBuilder);

        RefreshTokenBuilder.BuildModel(modelBuilder);

        ReviewModelBuilder.BuildModel(modelBuilder);


    }
}

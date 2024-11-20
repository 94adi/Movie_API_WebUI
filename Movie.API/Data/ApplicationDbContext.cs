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

    public DbSet<Genre> Genres {  get; set; }

    public DbSet<MovieGenre> MovieGenres { get; set; }

    public DbSet<MovieCarousel> MovieCarousels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ModelBuilder(modelBuilder);
    }

    private void ModelBuilder(ModelBuilder modelBuilder)
    {
        MovieModelBuilder.BuildModel(modelBuilder);

        RefreshTokenBuilder.BuildModel(modelBuilder);

        ReviewModelBuilder.BuildModel(modelBuilder);

        GenreModelBuilder.BuildModel(modelBuilder);

        MovieGenreModelBuilder.BuildModel(modelBuilder);

        MovieCarouselModelBuilder.BuildModel(modelBuilder);
    }
}

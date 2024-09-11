using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Movie.API.Models;

namespace Movie.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): 
        base(options)
    {
        
    }

    public DbSet<Movie.API.Models.Movie> Movies { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Models.Movie>()
            .HasKey(m => m.Id);

        modelBuilder.Entity<RefreshToken>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Models.Movie>()
            .Property(m => m.ReleaseDate)
            .HasConversion(
                p => p.ToDateTime(TimeOnly.MinValue),
                p => DateOnly.FromDateTime(p)
            );

        modelBuilder.Entity<Models.Movie>()
            .Property(m => m.CreatedDate)
            .HasColumnType("datetime2");

        modelBuilder.Entity<Models.Movie>()
            .Property(m => m.LatestUpdateDate)
            .HasColumnType("datetime2");

        modelBuilder.Entity<Models.Movie>()
            .Property(m => m.Rating)
            .HasColumnType("decimal(3,1)");
    }
}

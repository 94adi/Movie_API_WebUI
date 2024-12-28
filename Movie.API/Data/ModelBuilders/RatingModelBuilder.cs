namespace Movie.API.Data.ModelBuilders;

public static class RatingModelBuilder
{
    public static void BuildModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rating>().HasKey(x => x.Id);

        modelBuilder.Entity<Rating>()
            .HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Movie)
            .WithMany(m => m.Ratings)
            .HasForeignKey(r => r.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}

namespace Movie.API.Data.ModelBuilders;

public static class MovieCarouselModelBuilder
{
    public static void BuildModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.MovieCarousel>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Models.Movie>()
            .HasOne<Models.MovieCarousel>()
            .WithOne(mc => mc.Movie)
            .HasForeignKey<Models.MovieCarousel>(mc => mc.MovieId);
    }
}

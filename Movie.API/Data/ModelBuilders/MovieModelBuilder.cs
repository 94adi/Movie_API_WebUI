namespace Movie.API.Data.ModelBuilders
{
    public static class MovieModelBuilder
    {
        public static void BuildModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Movie>()
            .HasKey(m => m.Id);

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
                .Ignore(m => m.FinalScore);

            modelBuilder.Entity<Models.Movie>()
                .Ignore(m => m.ImageUrl);
        }
    }
}

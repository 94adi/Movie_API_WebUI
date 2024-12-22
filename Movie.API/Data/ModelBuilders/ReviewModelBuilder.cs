namespace Movie.API.Data.ModelBuilders;

public static class ReviewModelBuilder
{
    public static void BuildModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.Review>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Models.Review>()
            .Property(r => r.CreatedAt)
            .HasColumnType("datetime2");

        modelBuilder.Entity<Models.Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Models.Review>()
            .HasOne(r => r.Movie)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.MovieId);
    }
}

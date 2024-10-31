namespace Movie.API.Data.ModelBuilders;

public static class GenreModelBuilder
{
    public static void BuildModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>()
            .HasKey(x => x.Id);
    }
}

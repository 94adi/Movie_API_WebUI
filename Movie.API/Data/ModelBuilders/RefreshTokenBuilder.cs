namespace Movie.API.Data.ModelBuilders;

public static class RefreshTokenBuilder
{
    public static void BuildModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>()
            .HasKey(p => p.Id);
    }
}

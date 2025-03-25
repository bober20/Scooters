namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasMany(u => u.Reservations).WithOne(r => r.User);
        builder.HasMany(u => u.Rides).WithOne(r => r.User);
    }
}
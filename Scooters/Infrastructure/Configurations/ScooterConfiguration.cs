namespace Infrastructure.Configurations;

internal class ScooterConfiguration : IEntityTypeConfiguration<Scooter>
{
    public void Configure(EntityTypeBuilder<Scooter> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.ModelDescription);
        builder.OwnsOne(s => s.Coordinates);
        builder.HasMany(s => s.Reservations).WithOne(s => s.Scooter);
        builder.HasMany(s => s.Rides).WithOne(s => s.Scooter);
    }
}
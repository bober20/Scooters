namespace Infrastructure.Configurations;

internal class RideConfiguration : IEntityTypeConfiguration<Ride>
{
    public void Configure(EntityTypeBuilder<Ride> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.StartTime);
        builder.Property(r => r.EndTime);
        builder.Property(r => r.IsActive)
            .HasDefaultValue(true);
        builder.HasOne(r => r.Scooter)
            .WithMany()
            .HasForeignKey(r => r.ScooterId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
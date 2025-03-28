namespace Infrastructure.Configurations;

internal class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.ReservationEndTime);
        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);
        builder.HasOne(r => r.Scooter)
            .WithMany()
            .HasForeignKey(r => r.ScooterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
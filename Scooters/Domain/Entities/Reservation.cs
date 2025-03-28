namespace Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ScooterId { get; set; }
    public DateTime ReservationEndTime { get; set; }
    
    public User? User { get; set; }
    public Scooter? Scooter { get; set; }
}
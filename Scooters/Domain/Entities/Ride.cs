namespace Domain.Entities;

public class Ride
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ScooterId { get; set; }
    public DateTime RideStartTime { get; set; } 
    public DateTime? RideEndTime { get; set; }
    public bool IsActive { get; set; } = true;
    public User? User { get; set; }
    public Scooter? Scooter { get; set; }
}
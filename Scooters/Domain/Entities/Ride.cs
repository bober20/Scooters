namespace Domain.Entities;

public class Ride
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ScooterId { get; set; }
    public DateTime StartTime { get; set; } 
    public DateTime? EndTime { get; set; }
    public bool IsActive { get; set; } = true;
    public User? User { get; set; }
    public Scooter? Scooter { get; set; }
}
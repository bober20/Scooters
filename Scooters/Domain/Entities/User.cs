namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    public List<Reservation> Reservations { get; set; }
    public List<Ride> Rides { get; set; }
}
using Domain.Abstractions;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    public List<Reservation> Reservations { get; set; }
    public List<Ride> Rides { get; set; }
    
    private User() { }
    
    public User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
    }
    
    public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.IsCorrectPassword(password, PasswordHash);
    }
}
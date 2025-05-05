using Domain.Abstractions;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string ImageName { get; set; }
    
    public List<Reservation>? Reservations { get; set; }
    public List<Ride>? Rides { get; set; }
    
    public User() { }
    
    public User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
    }
    
    public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.IsCorrectPassword(password, PasswordHash);
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }

    public void DeleteImage()
    {
        ImageName = string.Empty;
    }
    
    public void SetImage(string imageName)
    {
        ImageName = imageName;
    }
}
namespace Application.Interfaces.Repositories;

public interface IRideRepository
{
    Task<Ride?> GetRidesByUserIdAsync();
}
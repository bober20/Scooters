namespace Application.Interfaces.Repositories;

public interface IRideRepository
{
    Task<Ride?> GetRideById(Guid id);
    Task<List<Ride>?> GetRidesByScooterIdAsync(Guid scooterId);
    Task<List<Ride>?> GetRidesByUserIdAsync(Guid userId);
    Task CreateRideAsync(Ride ride);
    Task DeleteRideAsync(Guid rideId);
    Task UpdateRideAsync(Ride ride);
}
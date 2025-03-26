namespace Application.Interfaces.Repositories;

public interface IRideRepository
{
    Task<Ride?> GetRideById(Guid id);
    Task<List<Ride>?> GetRidesByScooterIdAsync(Guid scooterId);
    Task<List<Ride>?> GetRidesByUserIdAsync(Guid userId);
    Task<Guid> CreateRideAsync(Ride ride);
    Task<Guid> DeleteRideAsync(Guid rideId);
    Task<Ride> UpdateRideAsync(Ride ride);
}
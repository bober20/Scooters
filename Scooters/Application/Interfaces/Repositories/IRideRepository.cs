namespace Application.Interfaces.Repositories;

public interface IRideRepository
{
    Task<Ride?> GetRidesByUserIdAsync(Guid userId);
    Task CreateRideAsync(Ride ride);
    Task DeleteRideAsync(Guid rideId);
}
namespace Application.Interfaces.Repositories;

public interface IRideRepository
{
    Task<Ride?> GetRidesByUserIdAsync();
    Task CreateRideAsync(Ride ride);
    Task DeleteRideAsync(Guid rideId);
}
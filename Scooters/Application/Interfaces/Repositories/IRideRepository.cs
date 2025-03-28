using System.Linq.Expressions;

namespace Application.Interfaces.Repositories;

public interface IRideRepository
{
    Task<Ride?> GetRideAsync(Guid id);
    Task<List<Ride>?> GetRidesAsync(Expression<Func<Ride, bool>> filter);
    Task<Ride?> GetRideAsync(Expression<Func<Ride, bool>> filter);
    Task<Ride> CreateRideAsync(Ride ride);
    Task DeleteRideAsync(Guid rideId);
    Task EndRideAsync(Guid rideId);
}
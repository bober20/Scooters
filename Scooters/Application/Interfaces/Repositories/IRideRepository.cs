using System.Linq.Expressions;

namespace Application.Interfaces.Repositories;

public interface IRideRepository
{
    Task<Ride?> GetRideByIdOrDefaultAsync(Guid id);
    Task<List<Ride>?> GetRidesByFilterOrDefaultAsync(Expression<Func<Ride, bool>> filter);
    Task<Ride?> GetSingleRideByFilterOrDefaultAsync(Expression<Func<Ride, bool>> filter);
    Task<Guid> CreateRideAsync(Ride ride);
    Task<Guid> DeleteRideAsync(Guid rideId);
    // Task<Ride> UpdateRideAsync(Ride ride);
    Task<Guid> EndRideAsync(Guid rideId);
}
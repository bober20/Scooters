using System.Linq.Expressions;

namespace Application.Interfaces.Repositories;

public interface IRideRepository
{
    Task<Ride?> GetRideByIdAsync(Guid id);
    Task<List<Ride>?> GetRidesByFilterAsync(Expression<Func<Ride, bool>> filter);

    Task<Guid> CreateRideAsync(Ride ride);
    Task<Guid> DeleteRideAsync(Guid rideId);
    Task<Ride> UpdateRideAsync(Ride ride);
}
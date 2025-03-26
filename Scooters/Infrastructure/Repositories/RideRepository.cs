using Infrastructure.Common.Persistence;

namespace Infrastructure.Repositories;

public class RideRepository : IRideRepository
{
    private readonly ScootersDbContext _dbContext;
    
    public RideRepository(ScootersDbContext context)
    {
        _dbContext = context;
    }
    
    public async Task<Ride?> GetRideById(Guid id)
    {
        var ride = await _dbContext.Rides
            .SingleOrDefaultAsync(x => x.Id == id);
        return ride;
    }

    public async Task<List<Ride>?> GetRidesByScooterIdAsync(Guid scooterId)
    {
        var rides = await _dbContext.Rides
            .AsNoTracking()
            .Where(r => r.ScooterId == scooterId)
            .ToListAsync();
        return rides;
    }

    public async Task<List<Ride>?> GetRidesByUserIdAsync(Guid userId)
    {
        var rides = await _dbContext.Rides
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .ToListAsync();
        return rides;
    }

    public async Task CreateRideAsync(Ride ride)
    {
        await _dbContext.Rides.AddAsync(ride);
    }

    public async Task DeleteRideAsync(Guid rideId)
    {
        var rideEntity = await _dbContext.Rides.FindAsync(rideId);
        if (rideEntity is not null)
        {
            _dbContext.Rides.Remove(rideEntity);
        }
    }

    public async Task UpdateRideAsync(Ride ride)
    {
        var rideEntity = await _dbContext.Rides.FindAsync(ride.Id);
        if (rideEntity is not null)
        {
            rideEntity.ScooterId = ride.ScooterId;
            rideEntity.UserId = ride.UserId;
            rideEntity.RideStartTime = ride.RideStartTime;
        }
    }
}
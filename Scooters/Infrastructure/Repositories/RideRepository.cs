using System.Linq.Expressions;
using Infrastructure.Common.Persistence;

namespace Infrastructure.Repositories;

public class RideRepository : IRideRepository
{
    private readonly ScootersDbContext _dbContext;
    
    public RideRepository(ScootersDbContext context)
    {
        _dbContext = context;
    }
    
    public async Task<Ride?> GetRideByIdOrDefaultAsync(Guid id)
    {
        var ride = await _dbContext.Rides
            .SingleOrDefaultAsync(x => x.Id == id);
        return ride;
    }

    public async Task<List<Ride>?> GetRidesByFilterOrDefaultAsync(Expression<Func<Ride, bool>> filter)
    {
        var rides = await _dbContext.Rides
            .AsNoTracking()
            .Where(filter)
            .ToListAsync();
        return rides;
    }

    public async Task<Ride?> GetSingleRideByFilterOrDefaultAsync(Expression<Func<Ride, bool>> filter)
    {
        var ride = await _dbContext.Rides
            .AsNoTracking()
            .Where(filter)
            .FirstOrDefaultAsync();
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

    public async Task<Guid> CreateRideAsync(Ride ride)
    {
        var addedRide = await _dbContext.Rides.AddAsync(ride);
        return addedRide.Entity.Id;
    }

    public async Task<Guid> DeleteRideAsync(Guid rideId)
    {
        var rideEntity = await _dbContext.Rides.FindAsync(rideId);
        if (rideEntity is not null)
        {
            _dbContext.Rides.Remove(rideEntity);
        }
        
        throw new KeyNotFoundException("Ride not found");
    }
    
    public async Task<Guid> EndRideAsync(Guid rideId)
    {
        var rideEntity = await _dbContext.Rides.FindAsync(rideId);
        if (rideEntity is not null)
        {
            rideEntity.RideEndTime = DateTime.Now;
            rideEntity.IsActive = false;
            return rideEntity.Id;
        }
        
        throw new KeyNotFoundException("Ride not found");
    }

    // public async Task<Ride> UpdateRideAsync(Ride ride)
    // {
    //     var rideEntity = await _dbContext.Rides.FindAsync(ride.Id);
    //     if (rideEntity is not null)
    //     {
    //         rideEntity.ScooterId = ride.ScooterId;
    //         rideEntity.UserId = ride.UserId;
    //         rideEntity.RideStartTime = ride.RideStartTime;
    //         rideEntity.RideEndTime = ride.RideEndTime;
    //         rideEntity.IsActive = ride.IsActive;
    //         return rideEntity;
    //     }
    //     
    //     throw new KeyNotFoundException("Ride not found");
    // }
}
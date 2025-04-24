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
    
    public async Task<Ride?> GetRideAsync(Guid id)
    {
        var ride = await _dbContext.Rides
            .SingleOrDefaultAsync(x => x.Id == id);
        if (ride is not null)
        {
            await _dbContext.Entry(ride).Reference(r => r.Scooter).LoadAsync();
        }
        return ride;
    }
    
    public async Task<Ride?> GetRideByScooterAsync(Guid scooterId)
    {
        var ride = await _dbContext.Rides
            .FirstOrDefaultAsync(r => r.ScooterId == scooterId && r.IsActive);
        return ride;
    }

    public async Task<Ride?> GetRideByUserAsync(Guid userId)
    {
        var ride = await _dbContext.Rides
            .FirstOrDefaultAsync(r => r.UserId == userId && r.IsActive);
        return ride;
    }

    public async Task<List<Ride>> GetRidesAsync(Expression<Func<Ride, bool>> filter)
    {
        var rides = await _dbContext.Rides
            .Where(filter)
            .ToListAsync();
        foreach (var ride in rides)
        {
            await _dbContext.Entry(ride).Reference(r => r.Scooter).LoadAsync();
        }
        return rides;
    }

    public async Task<Ride?> GetRideAsync(Expression<Func<Ride, bool>> filter)
    {
        var ride = await _dbContext.Rides
            .AsNoTracking()
            .Where(filter)
            .FirstOrDefaultAsync();
        return ride;
    }

    public async Task<Ride> CreateRideAsync(Ride ride)
    {
        var newRide = await _dbContext.Rides.AddAsync(ride);
        return newRide.Entity;
    }

    public async Task DeleteRideAsync(Guid rideId)
    {
        var rideEntity = await _dbContext.Rides.FindAsync(rideId);
        if (rideEntity is not null)
        {
            _dbContext.Rides.Remove(rideEntity);
        }
    }
    
    public async Task EndRideAsync(Guid rideId)
    {
        var rideEntity = await _dbContext.Rides.FindAsync(rideId);
        if (rideEntity is not null)
        {
            rideEntity.EndTime = DateTime.Now;
            rideEntity.IsActive = false;
        }
    }
}
using Infrastructure.Common.Persistence;

namespace Infrastructure.Repositories;

public class ScooterRepository : IScooterRepository
{
    private readonly ScootersDbContext _dbContext;
    
    public ScooterRepository(ScootersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Scooter>?> GetAllScootersAsync()
    {
        var scooters = await _dbContext.Scooters.AsNoTracking().ToListAsync();
        return scooters;
    }

    public async Task<Scooter?> GetScooterByIdAsync(Guid id)
    {
        var scooter = await _dbContext.Scooters
            .SingleOrDefaultAsync(s => s.Id == id);
        return scooter;
    }

    public async Task<Guid> CreateScooterAsync(Scooter scooter)
    {
        var addedScooter = await _dbContext.Scooters.AddAsync(scooter);
        return addedScooter.Entity.Id;
    }

    public async Task<Guid> DeleteScooterAsync(Guid id)
    {
        var scooterEntity = await _dbContext.Scooters.FindAsync(id);
        if (scooterEntity is not null)
        {
            _dbContext.Scooters.Remove(scooterEntity);
            return scooterEntity.Id;
        }
        
        throw new KeyNotFoundException("Ride not found");
    }

    public async Task<Scooter> UpdateScooterAsync(Scooter scooter)
    {
        var scooterEntity = await _dbContext.Scooters.FindAsync(scooter.Id);
        if (scooterEntity is not null)
        {
            scooterEntity.Coordinates = scooter.Coordinates;
            scooterEntity.ModelDescription = scooter.ModelDescription;
            return scooterEntity;
        }
        
        throw new KeyNotFoundException("Ride not found");
    }
}
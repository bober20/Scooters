using Infrastructure.Common.Persistence;

namespace Infrastructure.Repositories;

public class ScooterRepository : IScooterRepository
{
    private readonly ScootersDbContext _dbContext;
    
    public ScooterRepository(ScootersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Scooter?> GetScooterByIdAsync(Guid id)
    {
        var scooter = await _dbContext.Scooters
            .SingleOrDefaultAsync(s => s.Id == id);
        return scooter;
    }

    public async Task CreateScooterAsync(Scooter scooter)
    {
        await _dbContext.Scooters.AddAsync(scooter);
    }

    public async Task DeleteScooterAsync(Guid id)
    {
        var scooterEntity = await _dbContext.Scooters.FindAsync(id);
        if (scooterEntity is not null)
        {
            _dbContext.Scooters.Remove(scooterEntity);
        }
    }

    public async Task UpdateScooterAsync(Scooter scooter)
    {
        var scooterEntity = await _dbContext.Scooters.FindAsync(scooter.Id);
        if (scooterEntity is not null)
        {
            scooterEntity.Coordinates = scooter.Coordinates;
            scooterEntity.ModelDescription = scooter.ModelDescription;
        }
    }
}
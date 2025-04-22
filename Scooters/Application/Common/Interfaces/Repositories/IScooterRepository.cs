namespace Application.Common.Interfaces.Repositories;

public interface IScooterRepository
{
    Task<List<Scooter>> GetAvailableScootersAsync();
    Task<Scooter?> GetScooterAsync(Guid id);
    Task CreateScooterAsync(Scooter scooter);
    Task DeleteScooterAsync(Guid id);
    Task UpdateScooterAsync(Scooter scooter);
}
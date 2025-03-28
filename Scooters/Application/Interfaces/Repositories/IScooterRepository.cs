namespace Application.Interfaces.Repositories;

public interface IScooterRepository
{
    Task<List<Scooter>?> GetAllScootersAsync();
    Task<Scooter?> GetScooterAsync(Guid id);
    Task CreateScooterAsync(Scooter scooter);
    Task DeleteScooterAsync(Guid id);
    Task UpdateScooterAsync(Scooter scooter);
}
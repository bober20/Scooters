namespace Application.Interfaces.Repositories;

public interface IScooterRepository
{
    Task<Scooter?> GetScooterByIdAsync(Guid id);
    Task CreateScooterAsync(Scooter scooter);
    Task DeleteScooterAsync(Guid id);
    Task UpdateScooterAsync(Scooter scooter);
}
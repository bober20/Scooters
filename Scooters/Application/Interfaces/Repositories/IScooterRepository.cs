namespace Application.Interfaces.Repositories;

public interface IScooterRepository
{
    Task<List<Scooter>?> GetAllScootersAsync();
    Task<Scooter?> GetScooterByIdAsync(Guid id);
    Task<Guid> CreateScooterAsync(Scooter scooter);
    Task<Guid> DeleteScooterAsync(Guid id);
    Task<Scooter> UpdateScooterAsync(Scooter scooter);
}
namespace Application.Interfaces.Repositories;

public interface IScooterRepository
{
    Task<Scooter> GetScooterByIdAsync(Guid id);
    Task<Scooter> GetScootersAsync();
    Task CreateScooterAsync(Scooter scooter);
    
}
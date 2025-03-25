namespace Application.Interfaces.Repositories;

public interface IScooterRepository
{
    Task<ResponseData<Scooter>> GetScooterByIdAsync(Guid id);
    Task<ResponseData<List<Scooter>>> GetScootersAsync();
    Task CreateScooterAsync(Scooter scooter);
    
}
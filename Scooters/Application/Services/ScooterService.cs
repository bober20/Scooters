namespace Application.Services;

public class ScooterService
{
    private readonly IScooterRepository _scooterRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ScooterService(IScooterRepository scooterRepository, IUnitOfWork unitOfWork)
    {
        _scooterRepository = scooterRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<Scooter>> GetScooterByIdAsync(Guid scooterId)
    {
        try
        {
            var scooter = await _scooterRepository.GetScooterAsync(scooterId);
            return scooter is null 
                ? ResponseData<Scooter>.Failure("Scooter not found") 
                : ResponseData<Scooter>.Success(scooter);
        }
        catch (Exception e)
        {
            return ResponseData<Scooter>.Failure(e.Message);
        }
    }
    
    public async Task<ResponseData<List<Scooter>>> GetAllScootersAsync()
    {
        var scooters = await _scooterRepository.GetAllScootersAsync();
        return ResponseData<List<Scooter>>.Success(scooters);
    }
    
    public async Task UpdateScooterAsync(Scooter scooter)
    {
        await _scooterRepository.UpdateScooterAsync(scooter);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteScooterAsync(Guid scooterId)
    {
        await _scooterRepository.DeleteScooterAsync(scooterId);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task CreateScooterAsync(Scooter scooter)
    {
        await _scooterRepository.CreateScooterAsync(scooter);
        await _unitOfWork.SaveChangesAsync();
    }
}
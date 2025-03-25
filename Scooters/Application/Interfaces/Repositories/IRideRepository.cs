namespace Application.Interfaces.Repositories;

public interface IRideRepository
{
    Task<ResponseData<Ride>> GetRidesByUserIdAsync();
}
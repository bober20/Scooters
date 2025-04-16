using System.Linq.Expressions;

namespace Application.Services;

public class RideService
{
    private readonly IRideRepository _rideRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public RideService(IUnitOfWork unitOfWork, IRideRepository rideRepository, 
        IReservationRepository reservationRepository)
    {
        _unitOfWork = unitOfWork;
        _rideRepository = rideRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<ResponseData<List<Ride>>> GetRidesByFilterAsync(Expression<Func<Ride, bool>> filter)
    {
        var rides = await _rideRepository.GetRidesAsync(filter);
        return ResponseData<List<Ride>>.Success(rides);
    }

    public async Task<ResponseData<Ride>> GetRideByIdAsync(Guid rideId)
    {
        try
        {
            var ride = await _rideRepository.GetRideAsync(rideId);
            return ride is null 
                ? ResponseData<Ride>.Failure("Ride not found") 
                : ResponseData<Ride>.Success(ride);
        }
        catch(Exception ex)
        {
            return ResponseData<Ride>.Failure(ex.Message);
        }
    }
    
    public async Task<ResponseData<Guid>> CreateRideAsync(Ride ride)
    {
        try
        {
            var existingReservation = await _reservationRepository
                .GetReservationByScooterAsync(ride.ScooterId);
            
            var existingRide = await _rideRepository
                .GetRideByScooterAsync(ride.ScooterId);
            
            if (ride is not null || existingReservation is not null && existingReservation.UserId != ride.UserId)
            {
                return ResponseData<Guid>.Failure("Scooter is reserved");
            }

            if (existingReservation is not null)
            {
                await _reservationRepository.EndReservationAsync(existingReservation.Id);
            }
            
            var newRide = await _rideRepository.CreateRideAsync(ride);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Guid>.Success(newRide.Id);
        }
        catch(Exception ex)
        {
            return ResponseData<Guid>.Failure(ex.Message);
        }
    }

    public async Task EndRideAsync(Guid rideId)
    {
        await _rideRepository.EndRideAsync(rideId);
        await _unitOfWork.SaveChangesAsync();
    }
}
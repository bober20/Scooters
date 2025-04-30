namespace Application.Rides.Commands.CreateRide;

public class CreateRideHandler : IRequestHandler<CreateRideCommand, ResponseData<Ride>>
{
    private readonly IScooterRepository _scooterRepository;
    private readonly IRideRepository _rideRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateRideHandler(IRideRepository repository, IReservationRepository reservationRepository, 
        IScooterRepository scooterRepository, IUnitOfWork unitOfWork)
    {
        _rideRepository = repository;
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
        _scooterRepository = scooterRepository;
    }
    
    public async Task<ResponseData<Ride>> Handle(CreateRideCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingRide = await _rideRepository
                .GetRideByUserAsync(request.Ride.UserId);
            
            if (existingRide is not null)
            {
                return ResponseData<Ride>.Failure("Cancel current ride to start a new one");
            }
            
            var scooterReservation = await _reservationRepository
                .GetReservationByScooterAsync(request.Ride.ScooterId);
            
            var userReservation = await _reservationRepository
                .GetReservationByUserAsync(request.Ride.UserId);

            if (userReservation is not null && (scooterReservation is null || userReservation?.Id != scooterReservation?.Id))
            {
                return ResponseData<Ride>.Failure("You already have the reservation. Cancel it to start a ride or choose other scooter");
            }
            
            var ride = await _rideRepository
                .GetRideByScooterAsync(request.Ride.ScooterId);
            
            if (ride is not null || scooterReservation is not null && scooterReservation.UserId != request.Ride.UserId)
            {
                return ResponseData<Ride>.Failure("Scooter is reserved");
            }

            if (scooterReservation is not null)
            {
                await _reservationRepository.EndReservationAsync(scooterReservation.Id);
            }
            
            var newRide = await _rideRepository.CreateRideAsync(request.Ride);
            await _unitOfWork.SaveChangesAsync();
            newRide.Scooter = await _scooterRepository.GetScooterAsync(request.Ride.ScooterId);
            return ResponseData<Ride>.Success(newRide);
        }
        catch(Exception ex)
        {
            return ResponseData<Ride>.Failure(ex.Message);
        }
    }
}
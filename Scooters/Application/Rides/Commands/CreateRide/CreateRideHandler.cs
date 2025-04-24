namespace Application.Rides.Commands.CreateRide;

public class CreateRideHandler : IRequestHandler<CreateRideCommand, ResponseData<Guid>>
{
    private readonly IRideRepository _rideRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateRideHandler(IRideRepository repository, 
        IReservationRepository reservationRepository, 
        IUnitOfWork unitOfWork)
    {
        _rideRepository = repository;
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<Guid>> Handle(CreateRideCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingRide = await _rideRepository
                .GetRideByUserAsync(request.Ride.UserId);
            
            if (existingRide is not null)
            {
                return ResponseData<Guid>.Failure("Cancel current ride to start a new one");
            }
            
            var reservation = await _reservationRepository
                .GetReservationByScooterAsync(request.Ride.ScooterId);
            
            var ride = await _rideRepository
                .GetRideByScooterAsync(request.Ride.ScooterId);
            
            if (ride is not null || reservation is not null && reservation.UserId != request.Ride.UserId)
            {
                return ResponseData<Guid>.Failure("Scooter is reserved");
            }

            if (reservation is not null)
            {
                await _reservationRepository.EndReservationAsync(reservation.Id);
            }
            
            var newRide = await _rideRepository.CreateRideAsync(request.Ride);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Guid>.Success(newRide.Id);
        }
        catch(Exception ex)
        {
            return ResponseData<Guid>.Failure(ex.Message);
        }
    }
}
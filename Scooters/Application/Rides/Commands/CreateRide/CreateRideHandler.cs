namespace Application.Rides.Commands.CreateRide;

public class CreateRideHandler : IRequestHandler<CreateRideCommand, ResponseData<Ride>>
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
    
    public async Task<ResponseData<Ride>> Handle(CreateRideCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var scooterReservation = await _reservationRepository
                .GetReservationAsync(request.Ride.UserId, request.Ride.UserId);
            
            if (scooterReservation is not null)
            {
                await _reservationRepository.EndReservationAsync(scooterReservation.Id);
            }
            
            var ride = await _rideRepository.CreateRideAsync(request.Ride);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Ride>.Success(ride);
        }
        catch(Exception ex)
        {
            return ResponseData<Ride>.Failure(ex.Message);
        }
    }
}
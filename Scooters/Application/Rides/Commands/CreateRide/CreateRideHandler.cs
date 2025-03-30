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
            var reservation = await _reservationRepository
                .GetReservationAsync(request.Ride.UserId, request.Ride.ScooterId);
            
            if (reservation is not null)
            {
                await _reservationRepository.EndReservationAsync(reservation.Id);
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
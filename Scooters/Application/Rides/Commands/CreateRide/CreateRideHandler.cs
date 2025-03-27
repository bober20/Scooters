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
            var existingThisUserReservationForThisBike = await _reservationRepository
                .GetSingleReservationByFilterOrDefaultAsync(
                    r => r.ScooterId == request.Ride.ScooterId
                         && r.UserId == request.Ride.UserId);
            
            if (existingThisUserReservationForThisBike is not null)
            {
                await _reservationRepository.DeleteReservationAsync(existingThisUserReservationForThisBike.Id);
            }
            
            request.Ride.RideStartTime = DateTime.Now;
            
            var guid = await _rideRepository.CreateRideAsync(request.Ride);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Guid>.Success(guid);
        }
        catch(Exception ex)
        {
            return ResponseData<Guid>.Failure(ex.Message);
        }
    }
}
namespace Application.Rides.Commands.UpdateRide;

public class UpdateRideHandler : IRequestHandler<UpdateRideCommand, ResponseData<Ride>>
{
    private readonly IRideRepository _rideRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateRideHandler(IRideRepository repository, IUnitOfWork unitOfWork)
    {
        _rideRepository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<Ride>> Handle(UpdateRideCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ride = await _rideRepository.UpdateRideAsync(request.Ride);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Ride>.Success(ride);
        }
        catch(Exception ex)
        {
            return ResponseData<Ride>.Failure(ex.Message);
        }
    }
}
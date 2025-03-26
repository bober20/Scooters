namespace Application.Rides.Commands.CreateRide;

public class CreateRideHandler : IRequestHandler<CreateRideCommand, ResponseData<Guid>>
{
    private readonly IRideRepository _rideRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateRideHandler(IRideRepository repository, IUnitOfWork unitOfWork)
    {
        _rideRepository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<Guid>> Handle(CreateRideCommand request, CancellationToken cancellationToken)
    {
        try
        {
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
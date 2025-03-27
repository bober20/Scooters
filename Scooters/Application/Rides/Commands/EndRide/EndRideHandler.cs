namespace Application.Rides.Commands.EndRide;

public class EndRideHandler : IRequestHandler<EndRideCommand, ResponseData<Guid>>
{
    private readonly IRideRepository _rideRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public EndRideHandler(IRideRepository repository, IUnitOfWork unitOfWork)
    {
        _rideRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseData<Guid>> Handle(EndRideCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ride = await _rideRepository.EndRideAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Guid>.Success(ride);
        }
        catch(Exception ex)
        {
            return ResponseData<Guid>.Failure(ex.Message);
        }
    }
}
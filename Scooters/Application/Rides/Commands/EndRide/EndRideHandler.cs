namespace Application.Rides.Commands.EndRide;

public class EndRideHandler : IRequestHandler<EndRideCommand>
{
    private readonly IRideRepository _rideRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public EndRideHandler(IRideRepository repository, IUnitOfWork unitOfWork)
    {
        _rideRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EndRideCommand request, CancellationToken cancellationToken)
    {
        await _rideRepository.EndRideAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
namespace Application.Rides.Commands.DeleteRide;

public class DeleteRideHandler : IRequestHandler<DeleteRideCommand>
{
    private readonly IRideRepository _rideRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteRideHandler(IRideRepository repository, IUnitOfWork unitOfWork)
    {
        _rideRepository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(DeleteRideCommand request, CancellationToken cancellationToken)
    {
        await _rideRepository.DeleteRideAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
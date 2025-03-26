namespace Application.Rides.Commands.DeleteRide;

public class DeleteRideHandler : IRequestHandler<DeleteRideCommand, ResponseData<Guid>>
{
    private readonly IRideRepository _rideRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteRideHandler(IRideRepository repository, IUnitOfWork unitOfWork)
    {
        _rideRepository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<Guid>> Handle(DeleteRideCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var guid = await _rideRepository.DeleteRideAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Guid>.Success(guid);
        }
        catch(Exception ex)
        {
            return ResponseData<Guid>.Failure(ex.Message);
        }
    }
}
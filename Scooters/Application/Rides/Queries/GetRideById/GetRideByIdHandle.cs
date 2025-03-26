namespace Application.Rides.Queries.GetRideById;

public class GetRideByIdHandle : IRequestHandler<GetRideByIdQuery, ResponseData<Ride>>
{
    private readonly IRideRepository _rideRepository;
    
    public GetRideByIdHandle(IRideRepository repository)
    {
        _rideRepository = repository;
    }
    
    public async Task<ResponseData<Ride>> Handle(GetRideByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ride = await _rideRepository.GetRideByIdAsync(request.Id);
            return ride is null 
                ? ResponseData<Ride>.Failure("No ride with this id") 
                : ResponseData<Ride>.Success(ride);
        }
        catch(Exception ex)
        {
            return ResponseData<Ride>.Failure(ex.Message);
        }
    }
}
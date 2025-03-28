namespace Application.Rides.Queries.GetRideById;

public class GetRideHandle : IRequestHandler<GetRideQuery, ResponseData<Ride>>
{
    private readonly IRideRepository _rideRepository;
    
    public GetRideHandle(IRideRepository repository)
    {
        _rideRepository = repository;
    }
    
    public async Task<ResponseData<Ride>> Handle(GetRideQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ride = await _rideRepository.GetRideAsync(request.Id);
            return ResponseData<Ride>.Success(ride);
        }
        catch(Exception ex)
        {
            return ResponseData<Ride>.Failure(ex.Message);
        }
    }
}
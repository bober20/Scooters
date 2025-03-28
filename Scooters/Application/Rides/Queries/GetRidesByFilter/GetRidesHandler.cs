namespace Application.Rides.Queries.GetRidesByFilter;

public class GetRidesHandler : IRequestHandler<GetRidesQuery, ResponseData<List<Ride>>>
{
    private readonly IRideRepository _rideRepository;
    
    public GetRidesHandler(IRideRepository repository)
    {
        _rideRepository = repository;
    }
    
    public async Task<ResponseData<List<Ride>>> Handle(GetRidesQuery request, CancellationToken cancellationToken)
    {
        var rides = await _rideRepository.GetRidesAsync(request.Filter);
        return ResponseData<List<Ride>>.Success(rides);
    }
}
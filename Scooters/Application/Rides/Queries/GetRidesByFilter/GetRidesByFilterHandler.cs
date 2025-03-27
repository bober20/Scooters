namespace Application.Rides.Queries.GetRidesByFilter;

public class GetRidesByFilterHandler : IRequestHandler<GetRidesByFilterQuery, ResponseData<List<Ride>>>
{
    private readonly IRideRepository _rideRepository;
    
    public GetRidesByFilterHandler(IRideRepository repository)
    {
        _rideRepository = repository;
    }
    
    public async Task<ResponseData<List<Ride>>> Handle(GetRidesByFilterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var rides = await _rideRepository.GetRidesByFilterOrDefaultAsync(request.Filter);
            return rides is null
                ? ResponseData<List<Ride>>.Failure("No rides found") 
                : ResponseData<List<Ride>>.Success(rides);
        }
        catch(Exception ex)
        {
            return ResponseData<List<Ride>>.Failure(ex.Message);
        }
    }
}
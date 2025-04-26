namespace Application.Rides.Queries.GetRideByUserId;

public class GetRideHandler: IRequestHandler<GetRideQuery, ResponseData<Ride>>
{
    
    private readonly IRideRepository _rideRepository;
    
    public GetRideHandler(IRideRepository repository)
    {
        _rideRepository = repository;
    }
    
    public async Task<ResponseData<Ride>> Handle(GetRideQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ride = await _rideRepository.GetRideByUserAsync(request.UserId);
            return ride is not null ? ResponseData<Ride>.Success(ride) : 
                ResponseData<Ride>.Failure("There is no ride for this user");
        }
        catch(Exception ex)
        {
            return ResponseData<Ride>.Failure(ex.Message);
        }
    }
}
namespace Application.Scooters.Queries.GetAllScooters;

public class GetAvailableScootersHandler : IRequestHandler<GetAvailableScootersQuery, ResponseData<List<Scooter>>>
{
    private readonly IScooterRepository _scooterRepository;
    private readonly IRideRepository _rideRepository;
    private readonly IReservationRepository _reservationRepository;
    
    public GetAvailableScootersHandler(IScooterRepository scooterRepository, IRideRepository rideRepository, 
        IReservationRepository reservationRepository)
    {
        _scooterRepository = scooterRepository;
        _rideRepository = rideRepository;
        _reservationRepository = reservationRepository;
    }
    
    public async Task<ResponseData<List<Scooter>>> Handle(GetAvailableScootersQuery request, CancellationToken cancellationToken)
    {
        var scooters = await _scooterRepository.GetAvailableScootersAsync();
        var rides = await _rideRepository.GetRidesAsync(r => r.IsActive);
        var reservations = await _reservationRepository.GetReservationsAsync(r => r.IsActive);
        
        var availableScooters = scooters.Where(
            s => !rides.Any(r => r.ScooterId == s.Id) && 
                 !reservations.Any(r => r.ScooterId == s.Id)).ToList();
        return ResponseData<List<Scooter>>.Success(availableScooters);
    }
}
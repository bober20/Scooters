namespace Application.Reservations.Queries.GetReservationByFilter;

public class GetReservationByFilterHandler : IRequestHandler<GetReservationByFilterQuery, ResponseData<List<Reservation>>>
{
    private readonly IReservationRepository _reservationRepository;
    
    public GetReservationByFilterHandler(IReservationRepository repository)
    {
        _reservationRepository = repository;
    }
    
    public async Task<ResponseData<List<Reservation>>> Handle(GetReservationByFilterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reservations = await _reservationRepository.GetReservationsByFilterOrDefaultAsync(request.Filter);
            return reservations is null
                ? ResponseData<List<Reservation>>.Failure("No rides found") 
                : ResponseData<List<Reservation>>.Success(reservations);
        }
        catch(Exception ex)
        {
            return ResponseData<List<Reservation>>.Failure(ex.Message);
        }
    }
}
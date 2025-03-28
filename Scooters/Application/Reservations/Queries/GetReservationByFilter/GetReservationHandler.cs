namespace Application.Reservations.Queries.GetReservationByFilter;

public class GetReservationHandler : IRequestHandler<GetReservationQuery, ResponseData<List<Reservation>>>
{
    private readonly IReservationRepository _reservationRepository;
    
    public GetReservationHandler(IReservationRepository repository)
    {
        _reservationRepository = repository;
    }
    
    public async Task<ResponseData<List<Reservation>>> Handle(GetReservationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reservations = await _reservationRepository.GetReservationsAsync(request.Filter);
            return ResponseData<List<Reservation>>.Success(reservations);
        }
        catch(Exception ex)
        {
            return ResponseData<List<Reservation>>.Failure(ex.Message);
        }
    }
}
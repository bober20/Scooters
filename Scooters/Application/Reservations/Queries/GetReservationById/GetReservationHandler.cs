namespace Application.Reservations.Queries.GetReservationById;

public class GetReservationHandler : IRequestHandler<GetReservationQuery, ResponseData<Reservation>>
{
    private readonly IReservationRepository _reservationRepository;
    
    public GetReservationHandler(IReservationRepository repository)
    {
        _reservationRepository = repository;
    }
    
    public async Task<ResponseData<Reservation>> Handle(GetReservationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reservation = await _reservationRepository.GetReservationAsync(request.Id);
            return ResponseData<Reservation>.Success(reservation);
        }
        catch(Exception ex)
        {
            return ResponseData<Reservation>.Failure(ex.Message);
        }
    }
}
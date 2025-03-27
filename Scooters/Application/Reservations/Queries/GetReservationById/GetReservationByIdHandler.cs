namespace Application.Reservations.Queries.GetReservationById;

public class GetReservationByIdHandler : IRequestHandler<GetReservationByIdQuery, ResponseData<Reservation>>
{
    private readonly IReservationRepository _reservationRepository;
    
    public GetReservationByIdHandler(IReservationRepository repository)
    {
        _reservationRepository = repository;
    }
    
    public async Task<ResponseData<Reservation>> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reservation = await _reservationRepository.GetReservationByIdOrDefaultAsync(request.Id);
            return reservation is null
                ? ResponseData<Reservation>.Failure("No reservation found") 
                : ResponseData<Reservation>.Success(reservation);
        }
        catch(Exception ex)
        {
            return ResponseData<Reservation>.Failure(ex.Message);
        }
    }
}
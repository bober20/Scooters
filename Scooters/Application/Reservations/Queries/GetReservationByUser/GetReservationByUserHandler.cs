namespace Application.Reservations.Queries.GetReservationByUser;

public class GetReservationByUserHandler : IRequestHandler<GetReservationByUserQuery, ResponseData<Reservation?>>
{
    private readonly IReservationRepository _reservationRepository;

    public GetReservationByUserHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<ResponseData<Reservation?>> Handle(GetReservationByUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reservation = await _reservationRepository.GetReservationByUserAsync(request.UserId);
            return reservation is not null 
                ? ResponseData<Reservation?>.Success(reservation)
                : ResponseData<Reservation?>.Failure("Reservation not found");
        }
        catch(Exception ex)
        {
            return ResponseData<Reservation?>.Failure(ex.Message);
        }
    }
}
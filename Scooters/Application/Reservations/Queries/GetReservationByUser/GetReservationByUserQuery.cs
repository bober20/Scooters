namespace Application.Reservations.Queries.GetReservationByUser;

public record GetReservationByUserQuery(Guid UserId) : IRequest<ResponseData<Reservation?>>;
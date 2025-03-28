namespace Application.Reservations.Queries.GetReservationById;

public record GetReservationQuery(Guid Id) : IRequest<ResponseData<Reservation>>;
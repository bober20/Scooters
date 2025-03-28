using System.Linq.Expressions;

namespace Application.Reservations.Queries.GetReservationByFilter;

public record GetReservationQuery(Expression<Func<Reservation, bool>> Filter) : IRequest<ResponseData<List<Reservation>>>;
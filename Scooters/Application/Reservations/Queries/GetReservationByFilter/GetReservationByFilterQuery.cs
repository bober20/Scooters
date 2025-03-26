using System.Linq.Expressions;

namespace Application.Reservations.Queries.GetReservationByFilter;

public record GetReservationByFilterQuery(Expression<Func<Reservation, bool>> Filter) : IRequest<ResponseData<List<Reservation>>>;
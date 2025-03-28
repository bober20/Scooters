using System.Linq.Expressions;

namespace Application.Rides.Queries.GetRidesByFilter;

public record GetRidesQuery(Expression<Func<Ride, bool>> Filter) : IRequest<ResponseData<List<Ride>>>;
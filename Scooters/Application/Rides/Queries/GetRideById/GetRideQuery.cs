namespace Application.Rides.Queries.GetRideById;

public record GetRideQuery(Guid Id) : IRequest<ResponseData<Ride>>;
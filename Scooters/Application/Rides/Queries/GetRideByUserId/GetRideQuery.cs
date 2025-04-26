namespace Application.Rides.Queries.GetRideByUserId;

public record GetRideQuery(Guid UserId) : IRequest<ResponseData<Ride>>;
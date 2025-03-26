namespace Application.Rides.Queries.GetRideById;

public record GetRideByIdQuery(Guid Id) : IRequest<ResponseData<Ride>>;
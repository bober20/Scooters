namespace Application.Rides.Commands.UpdateRide;

public record UpdateRideCommand(Ride Ride) : IRequest<ResponseData<Ride>>;
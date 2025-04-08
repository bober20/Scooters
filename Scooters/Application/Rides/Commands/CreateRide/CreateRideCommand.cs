namespace Application.Rides.Commands.CreateRide;

public record CreateRideCommand(Ride Ride) : IRequest<ResponseData<Guid>>;
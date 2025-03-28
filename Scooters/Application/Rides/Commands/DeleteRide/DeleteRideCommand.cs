namespace Application.Rides.Commands.DeleteRide;

public record DeleteRideCommand(Guid Id) : IRequest;
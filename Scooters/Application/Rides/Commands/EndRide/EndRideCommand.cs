namespace Application.Rides.Commands.EndRide;

public record EndRideCommand(Guid Id) : IRequest<ResponseData<Guid>>;
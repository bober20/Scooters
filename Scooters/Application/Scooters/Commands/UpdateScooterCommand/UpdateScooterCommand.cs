namespace Application.Scooters.Commands.UpdateScooterCommand;

public record UpdateScooterCommand(Scooter Scooter) : IRequest<ResponseData<Scooter>>;
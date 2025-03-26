namespace Application.Scooters.Commands.CreateScooterCommand;

public record CreateScooterCommand(Scooter Scooter) : IRequest<ResponseData<Guid>>;
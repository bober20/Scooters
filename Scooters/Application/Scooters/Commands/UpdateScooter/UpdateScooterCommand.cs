namespace Application.Scooters.Commands.UpdateScooter;

public record UpdateScooterCommand(Scooter Scooter) : IRequest;
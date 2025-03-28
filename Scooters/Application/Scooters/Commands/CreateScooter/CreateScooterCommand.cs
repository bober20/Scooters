namespace Application.Scooters.Commands.CreateScooter;

public record CreateScooterCommand(Scooter Scooter) : IRequest;
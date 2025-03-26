namespace Application.Scooters.Commands.DeleteScooter;

public record DeleteScooterCommand(Guid Id) : IRequest<ResponseData<Guid>>;
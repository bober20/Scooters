namespace Application.Scooters.Commands.DeleteScooterCommand;

public record DeleteScooterCommand(Guid Id) : IRequest<ResponseData<Guid>>;
namespace Application.Reservations.Commands.DeleteReservation;

public record DeleteReservationCommand(Guid Id) : IRequest<ResponseData<Guid>>;
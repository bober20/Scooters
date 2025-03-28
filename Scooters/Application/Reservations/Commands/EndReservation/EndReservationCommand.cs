namespace Application.Reservations.Commands.EndReservation;

public record EndReservationCommand(Guid Id) : IRequest;
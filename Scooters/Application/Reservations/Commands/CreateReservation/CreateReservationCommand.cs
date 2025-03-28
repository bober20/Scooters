namespace Application.Reservations.Commands.CreateReservation;

public record CreateReservationCommand(Reservation Reservation) : IRequest;
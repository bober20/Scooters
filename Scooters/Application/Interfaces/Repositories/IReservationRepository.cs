namespace Application.Interfaces.Repositories;

public interface IReservationRepository
{
    Task<Ride?> GetRidesByByIdAsync(Guid id);
    Task<Reservation?> GetReservationByUserIdAsync(Guid userId);
    Task<Reservation?> GetReservationByScooterIdAsync(Guid scooterId);
    Task CreateReservationAsync(Reservation reservation);
    Task DeleteReservationAsync(Guid reservationId);
}
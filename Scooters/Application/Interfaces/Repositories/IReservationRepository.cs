namespace Application.Interfaces.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetReservationByIdAsync(Guid id);
    Task<List<Reservation>?> GetReservationsByUserIdAsync(Guid userId);
    Task<List<Reservation>?> GetReservationsByScooterIdAsync(Guid scooterId);
    Task<Guid> CreateReservationAsync(Reservation reservation);
    Task<Guid> DeleteReservationAsync(Guid reservationId);
}
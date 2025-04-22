using System.Linq.Expressions;

namespace Application.Common.Interfaces.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetReservationAsync(Guid id);
    Task<List<Reservation>> GetReservationsAsync(Expression<Func<Reservation, bool>> filter);
    Task<Reservation?> GetReservationAsync(Guid userId, Guid scooterId);
    Task<Reservation?> GetReservationByScooterAsync(Guid scooterId);
    Task<Reservation?> GetReservationByUserAsync(Guid scooterId);
    Task CreateReservationAsync(Reservation reservation);
    Task DeleteReservationAsync(Guid reservationId);
    Task EndReservationAsync(Guid reservationId);
    Task EndAllUserReservationsAsync(Guid userId);
}
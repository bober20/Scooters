using System.Linq.Expressions;

namespace Application.Interfaces.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetReservationByIdOrDefaultAsync(Guid id);
    Task<List<Reservation>?> GetReservationsByFilterOrDefaultAsync(Expression<Func<Reservation, bool>> filter);
    Task<Reservation?> GetSingleReservationByFilterOrDefaultAsync(Expression<Func<Reservation, bool>> filter);
    Task<Guid> CreateReservationAsync(Reservation reservation);
    Task<Guid> DeleteReservationAsync(Guid reservationId);
}
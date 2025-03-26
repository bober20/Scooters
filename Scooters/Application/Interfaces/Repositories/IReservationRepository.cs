using System.Linq.Expressions;

namespace Application.Interfaces.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetReservationByIdAsync(Guid id);
    Task<List<Reservation>?> GetReservationsByFilterAsync(Expression<Func<Reservation, bool>> filter);
    Task<Guid> CreateReservationAsync(Reservation reservation);
    Task<Guid> DeleteReservationAsync(Guid reservationId);
}
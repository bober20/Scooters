using System.Linq.Expressions;
using Infrastructure.Common.Persistence;

namespace Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ScootersDbContext _dbContext;
    
    public ReservationRepository(ScootersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Reservation?> GetReservationByIdOrDefaultAsync(Guid id)
    {
        var reservation = await _dbContext.Reservations
            .FirstOrDefaultAsync(s => s.Id == id);
        return reservation;
    }

    public async Task<List<Reservation>?> GetReservationsByFilterOrDefaultAsync(Expression<Func<Reservation, bool>> filter)
    {
        var reservations = await _dbContext.Reservations
            .AsNoTracking()
            .Where(filter)
            .ToListAsync();
        return reservations;
    }

    public async Task<Reservation?> GetSingleReservationByFilterOrDefaultAsync(Expression<Func<Reservation, bool>> filter)
    {
        var reservation = await _dbContext.Reservations
            .AsNoTracking()
            .Where(filter)
            .FirstOrDefaultAsync();
        return reservation;
    }

    public async Task<List<Reservation>?> GetReservationsByUserIdAsync(Guid userId)
    {
        var reservation = await _dbContext.Reservations
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .ToListAsync();
        return reservation;
    }

    public async Task<List<Reservation>?> GetReservationsByScooterIdAsync(Guid scooterId)
    {
        var reservation = await _dbContext.Reservations
            .AsNoTracking()
            .Where(s => s.ScooterId == scooterId)
            .ToListAsync();
        return reservation;
    }

    public async Task<Guid> CreateReservationAsync(Reservation reservation)
    {
        var addedReservation = await _dbContext.Reservations.AddAsync(reservation);
        return addedReservation.Entity.Id;
    }

    public async Task<Guid> DeleteReservationAsync(Guid reservationId)
    {
        var reservation = await _dbContext.Reservations.FindAsync(reservationId);
        if (reservation is not null)
        {
            _dbContext.Reservations.Remove(reservation);
            return reservation.Id;
        }

        throw new KeyNotFoundException("Reservation not found");
    }

    public async Task<Guid> EndReservationAsync(Guid reservationId)
    {
        var reservation = await _dbContext.Reservations.FindAsync(reservationId);
        if (reservation is not null)
        {
            reservation.IsActive = false;
            return reservation.Id;
        }
        
        throw new KeyNotFoundException("Reservation not found");
    }
}
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
    
    public async Task<Reservation?> GetReservationAsync(Guid id)
    {
        var reservation = await _dbContext.Reservations
            .SingleOrDefaultAsync(s => s.Id == id);
        if (reservation is not null)
        {
            await _dbContext.Entry(reservation).Reference(r => r.Scooter).LoadAsync();
        }
        return reservation;
    }

    public async Task<List<Reservation>?> GetReservationsAsync(Expression<Func<Reservation, bool>> filter)
    {
        var reservations = await _dbContext.Reservations
            .Where(filter)
            .ToListAsync();
        foreach (var reservation in reservations)
        {
            await _dbContext.Entry(reservation).Reference(r => r.Scooter).LoadAsync();
        }
        return reservations;
    }

    public async Task<Reservation?> GetReservationAsync(Guid userId, Guid scooterId)
    {
        var reservation = await _dbContext.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId && s.ScooterId == scooterId);
        return reservation;
    }
    
    public async Task CreateReservationAsync(Reservation reservation)
    {
        await _dbContext.Reservations.AddAsync(reservation);
    }

    public async Task DeleteReservationAsync(Guid reservationId)
    {
        var reservation = await _dbContext.Reservations.FindAsync(reservationId);
        if (reservation is not null)
        {
            _dbContext.Reservations.Remove(reservation);
        }
    }

    public async Task EndAllUserReservationsAsync(Guid userId)
    {
        var reservations = await _dbContext.Reservations
            .Where(r => r.UserId == userId && r.IsActive)
            .ToListAsync();
        foreach (var reservation in reservations)
        {
            reservation.IsActive = false;
        }
        _dbContext.SaveChanges();
    }

    public async Task EndReservationAsync(Guid reservationId)
    {
        var reservation = await _dbContext.Reservations.FindAsync(reservationId);
        if (reservation is not null)
        {
            reservation.IsActive = false;
        }
    }
}
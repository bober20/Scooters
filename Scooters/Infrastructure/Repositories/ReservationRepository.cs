using Infrastructure.Common.Persistence;

namespace Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ScootersDbContext _dbContext;
    
    public ReservationRepository(ScootersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Reservation?> GetReservationByIdAsync(Guid id)
    {
        var reservation = await _dbContext.Reservations
            .FirstOrDefaultAsync(s => s.Id == id);
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
}
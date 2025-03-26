namespace Application.Interfaces.Repositories;

public interface IReservationRepository
{
    Task<Ride> GetRidesByByIdAsync(Guid id);
    Task<Reservation> GetReservationByUserIdAsync();
    Task<Reservation> GetReservationByScooterIdAsync();
    Task CreateReservationAsync(Reservation reservation);
    Task DeleteReservationAsync(Guid reservationId);
}
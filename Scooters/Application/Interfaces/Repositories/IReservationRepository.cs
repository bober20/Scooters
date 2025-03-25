namespace Application.Interfaces.Repositories;

public interface IReservationRepository
{
    Task<ResponseData<Ride>> GetRidesByByIdAsync(Guid id);
    Task<ResponseData<Reservation>> GetReservationByUserIdAsync();
    Task<ResponseData<Reservation>> GetReservationByScooterIdAsync();
    Task CreateReservationAsync(Reservation reservation);
    Task DeleteReservationAsync(Guid reservationId);
}
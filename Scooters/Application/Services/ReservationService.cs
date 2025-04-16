using System.Linq.Expressions;

namespace Application.Services;

public class ReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public ReservationService(IReservationRepository repository, IUnitOfWork unitOfWork)
    {
        _reservationRepository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<bool>> CreateReservationAsync(Reservation reservation)
    {
        var existingReservations = await _reservationRepository.GetReservationsAsync(
            r => r.ScooterId == reservation.ScooterId && r.IsActive);
        if (existingReservations is not null && existingReservations.Count > 0)
        {
            return ResponseData<bool>.Failure("Scooter is already reserved");
        }
        await _reservationRepository.EndAllUserReservationsAsync(reservation.UserId);
        await _reservationRepository.CreateReservationAsync(reservation);
        await _unitOfWork.SaveChangesAsync();
        return ResponseData<bool>.Success(true);
    }
    
    public async Task EndReservationAsync(Guid reservationId)
    {
        await _reservationRepository.EndReservationAsync(reservationId);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<ResponseData<Reservation?>> GetReservationByUserIdAsync(Guid userId)
    {
        try
        {
            var reservation = await _reservationRepository.GetReservationByUserAsync(userId);
            return reservation is not null 
                ? ResponseData<Reservation?>.Success(reservation)
                : ResponseData<Reservation?>.Failure("Reservation not found");
        }
        catch(Exception ex)
        {
            return ResponseData<Reservation?>.Failure(ex.Message);
        }
    }
    
    public async Task<ResponseData<Reservation>> GetReservationByIdAsync(Guid reservationId)
    {
        try
        {
            var reservation = await _reservationRepository.GetReservationAsync(reservationId);
            return reservation is not null 
                ? ResponseData<Reservation>.Success(reservation)
                : ResponseData<Reservation>.Failure("Reservation not found");
        }
        catch(Exception ex)
        {
            return ResponseData<Reservation>.Failure(ex.Message);
        }
    }

    public async Task<ResponseData<List<Reservation>>> GetReservationsByFilterAsync(
        Expression<Func<Reservation, bool>> filter)
    {
        try
        {
            var reservations = await _reservationRepository.GetReservationsAsync(filter);
            return ResponseData<List<Reservation>>.Success(reservations);
        }
        catch(Exception ex)
        {
            return ResponseData<List<Reservation>>.Failure(ex.Message);
        }
    }
    
}
namespace Application.Reservations.Commands.CreateReservation;

public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, ResponseData<bool>>
{
    private readonly IReservationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateReservationHandler(IReservationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<bool>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var existingReservations = await _repository.GetReservationsAsync(
                r => r.ScooterId == request.Reservation.ScooterId && r.IsActive);
        if (existingReservations is not null && existingReservations.Count > 0)
        {
            return ResponseData<bool>.Failure("Scooter is already reserved");
        }
        await _repository.EndAllUserReservationsAsync(request.Reservation.UserId);
        await _repository.CreateReservationAsync(request.Reservation);
        await _unitOfWork.SaveChangesAsync();
        return ResponseData<bool>.Success(true);
    }
}
namespace Application.Reservations.Commands.DeleteReservation;

public class DeleteReservationHandler : IRequestHandler<DeleteReservationCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteReservationHandler(IReservationRepository repository, IUnitOfWork unitOfWork)
    {
        _reservationRepository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        await _reservationRepository.DeleteReservationAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
namespace Application.Reservations.Commands.EndReservation;

public class EndReservationHandler : IRequestHandler<EndReservationCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public EndReservationHandler(IReservationRepository reservationRepository, IUnitOfWork unitOfWork)
    {
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(EndReservationCommand request, CancellationToken cancellationToken)
    {
        await _reservationRepository.EndReservationAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
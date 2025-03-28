namespace Application.Reservations.Commands.CreateReservation;

public class CreateReservationHandler : IRequestHandler<CreateReservationCommand>
{
    private readonly IReservationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateReservationHandler(IReservationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        await _repository.CreateReservationAsync(request.Reservation);
        await _unitOfWork.SaveChangesAsync();
    }
}
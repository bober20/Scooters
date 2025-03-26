namespace Application.Reservations.Commands.CreateReservation;

public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, ResponseData<Guid>>
{
    private readonly IReservationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateReservationHandler(IReservationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<Guid>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var guid = await _repository.CreateReservationAsync(request.Reservation);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Guid>.Success(guid);
        }
        catch(Exception ex)
        {
            return ResponseData<Guid>.Failure(ex.Message);
        }
    }
}
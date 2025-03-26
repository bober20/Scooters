namespace Application.Reservations.Commands.DeleteReservation;

public class DeleteReservationHandler : IRequestHandler<DeleteReservationCommand, ResponseData<Guid>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteReservationHandler(IReservationRepository repository, IUnitOfWork unitOfWork)
    {
        _reservationRepository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<Guid>> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var guid = await _reservationRepository.DeleteReservationAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Guid>.Success(guid);
        }
        catch(Exception ex)
        {
            return ResponseData<Guid>.Failure(ex.Message);
        }
    }
}
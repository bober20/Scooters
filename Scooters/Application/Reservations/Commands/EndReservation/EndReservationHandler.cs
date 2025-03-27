namespace Application.Reservations.Commands.EndReservation;

public class EndReservationHandler : IRequestHandler<EndReservationCommand, ResponseData<Guid>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public EndReservationHandler(IReservationRepository reservationRepository, IUnitOfWork unitOfWork)
    {
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<Guid>> Handle(EndReservationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var guid = await _reservationRepository.EndReservationAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Guid>.Success(guid);
        }
        catch(Exception ex)
        {
            return ResponseData<Guid>.Failure(ex.Message);
        }
    }
}
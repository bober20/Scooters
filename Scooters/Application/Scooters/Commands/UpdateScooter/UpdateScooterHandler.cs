namespace Application.Scooters.Commands.UpdateScooter;

public class UpdateScooterHandler : IRequestHandler<UpdateScooterCommand>
{
    private readonly IScooterRepository _scooterRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateScooterHandler(IScooterRepository scooterRepository, IUnitOfWork unitOfWork)
    {
        _scooterRepository = scooterRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(UpdateScooterCommand request, CancellationToken cancellationToken)
    {
        await _scooterRepository.UpdateScooterAsync(request.Scooter);
        await _unitOfWork.SaveChangesAsync();
    }
}
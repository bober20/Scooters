namespace Application.Scooters.Commands.UpdateScooterCommand;

public class UpdateScooterHandler : IRequestHandler<UpdateScooterCommand, ResponseData<Scooter>>
{
    private readonly IScooterRepository _scooterRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateScooterHandler(IScooterRepository scooterRepository, IUnitOfWork unitOfWork)
    {
        _scooterRepository = scooterRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<Scooter>> Handle(UpdateScooterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var updatedScooter = await _scooterRepository.UpdateScooterAsync(request.Scooter);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Scooter>.Success(updatedScooter);
        }
        catch (Exception ex)
        {
            return ResponseData<Scooter>.Failure(ex.Message);
        }
    }
}
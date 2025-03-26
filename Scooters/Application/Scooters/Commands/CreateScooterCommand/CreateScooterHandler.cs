using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;

namespace Application.Scooters.Commands.CreateScooterCommand;

public class CreateScooterHandler : IRequestHandler<CreateScooterCommand, ResponseData<Guid>>
{
    private readonly IScooterRepository _scooterRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateScooterHandler(IScooterRepository scooterRepository, IUnitOfWork unitOfWork)
    {
        _scooterRepository = scooterRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<Guid>> Handle(CreateScooterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var guid = await _scooterRepository.CreateScooterAsync(request.Scooter);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Guid>.Success(guid);
        }
        catch(Exception ex)
        {
            return ResponseData<Guid>.Failure(ex.Message);
        }
    }
}
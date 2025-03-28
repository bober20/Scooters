using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;

namespace Application.Scooters.Commands.CreateScooter;

public class CreateScooterHandler : IRequestHandler<CreateScooterCommand>
{
    private readonly IScooterRepository _scooterRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateScooterHandler(IScooterRepository scooterRepository, IUnitOfWork unitOfWork)
    {
        _scooterRepository = scooterRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(CreateScooterCommand request, CancellationToken cancellationToken)
    {
        await _scooterRepository.CreateScooterAsync(request.Scooter);
        await _unitOfWork.SaveChangesAsync();
    }
}
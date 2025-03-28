using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;

namespace Application.Scooters.Commands.DeleteScooter;

public class DeleteScooterHandler : IRequestHandler<DeleteScooterCommand>
{
    private readonly IScooterRepository _scooterRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteScooterHandler(IScooterRepository scooterRepository, IUnitOfWork unitOfWork)
    {
        _scooterRepository = scooterRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(DeleteScooterCommand request, CancellationToken cancellationToken)
    {
        await _scooterRepository.DeleteScooterAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
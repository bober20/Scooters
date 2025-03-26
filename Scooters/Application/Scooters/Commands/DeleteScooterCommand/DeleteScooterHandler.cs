using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;

namespace Application.Scooters.Commands.DeleteScooterCommand;

public class DeleteScooterHandler : IRequestHandler<DeleteScooterCommand, ResponseData<Guid>>
{
    private readonly IScooterRepository _scooterRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteScooterHandler(IScooterRepository scooterRepository, IUnitOfWork unitOfWork)
    {
        _scooterRepository = scooterRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseData<Guid>> Handle(DeleteScooterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var deletedScooterGuid = await _scooterRepository.DeleteScooterAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<Guid>.Success(deletedScooterGuid);
        }
        catch (Exception ex)
        {
            return ResponseData<Guid>.Failure(ex.Message);
        }
    }
}
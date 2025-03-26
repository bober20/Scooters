using Application.Interfaces.Repositories;

namespace Application.Scooters.Queries.GetScooterById;

public class GetScooterByIdHandler : IRequestHandler<GetScooterByIdQuery, ResponseData<Scooter>>
{
    private readonly IScooterRepository _scooterRepository;
        
    public GetScooterByIdHandler(IScooterRepository scooterRepository)
    {
        _scooterRepository = scooterRepository;
    }
    
    public async Task<ResponseData<Scooter>> Handle(GetScooterByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var scooter = await _scooterRepository.GetScooterByIdAsync(request.Id);
            return scooter is null
                ? ResponseData<Scooter>.Failure("Scooter not found")
                : ResponseData<Scooter>.Success(scooter);
        }
        catch (Exception e)
        {
            return ResponseData<Scooter>.Failure(e.Message);
        }
    }
}
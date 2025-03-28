namespace Application.Scooters.Queries.GetScooterById;

public class GetScooterHandler : IRequestHandler<GetScooterQuery, ResponseData<Scooter>>
{
    private readonly IScooterRepository _scooterRepository;
        
    public GetScooterHandler(IScooterRepository scooterRepository)
    {
        _scooterRepository = scooterRepository;
    }
    
    public async Task<ResponseData<Scooter>> Handle(GetScooterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var scooter = await _scooterRepository.GetScooterAsync(request.Id);
            return ResponseData<Scooter>.Success(scooter);
        }
        catch (Exception e)
        {
            return ResponseData<Scooter>.Failure(e.Message);
        }
    }
}
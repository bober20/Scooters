namespace Application.Scooters.Queries.GetAllScooters;

public class GetAllScootersHandler : IRequestHandler<GetAllScootersQuery, ResponseData<List<Scooter>>>
{
    private readonly IScooterRepository _scooterRepository;
    
    public GetAllScootersHandler(IScooterRepository scooterRepository)
    {
        _scooterRepository = scooterRepository;
    }
    
    public async Task<ResponseData<List<Scooter>>> Handle(GetAllScootersQuery request, CancellationToken cancellationToken)
    {
        var scooters = await _scooterRepository.GetAllScootersAsync();
        return ResponseData<List<Scooter>>.Success(scooters);
    }
}
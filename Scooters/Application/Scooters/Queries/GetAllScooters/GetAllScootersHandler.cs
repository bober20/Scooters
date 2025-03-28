namespace Application.Scooters.Queries.GetAllScooters;

public class GetAllScootersHandler : IRequestHandler<GetAllScootersQuery, ResponseData<Scooter>>
{
    public Task<ResponseData<Scooter>> Handle(GetAllScootersQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
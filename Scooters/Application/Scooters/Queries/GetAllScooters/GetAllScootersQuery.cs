namespace Application.Scooters.Queries.GetAllScooters;

public record GetAllScootersQuery() : IRequest<ResponseData<List<Scooter>>>;
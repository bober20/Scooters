namespace Application.Scooters.Queries.GetAllScooters;

public record GetAvailableScootersQuery() : IRequest<ResponseData<List<Scooter>>>;
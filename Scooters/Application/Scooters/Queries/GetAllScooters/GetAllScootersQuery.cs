namespace Application.Scooters.Queries.GetAllScooters;

public record GetAllScootersQuery() : IRequest<ResponseData<Scooter>>;
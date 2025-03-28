namespace Application.Scooters.Queries.GetScooterById;

public record GetScooterQuery(Guid Id) : IRequest<ResponseData<Scooter>>;
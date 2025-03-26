namespace Application.Scooters.Queries.GetScooterById;

public record GetScooterByIdQuery(Guid Id) : IRequest<ResponseData<Scooter>>;
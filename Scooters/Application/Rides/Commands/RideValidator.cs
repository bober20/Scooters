using Application.Scooters.Queries.GetAllScooters;

namespace Application.Rides.Commands;

public class RideValidator : AbstractValidator<Ride>
{
    public RideValidator()
    {
        RuleFor(x => x.ScooterId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RideStartTime).NotEmpty().LessThan(DateTime.Now);
    }
}
namespace Application.Rides.Validators;

public class RideValidator : AbstractValidator<Ride>
{
    public RideValidator()
    {
        RuleFor(x => x.ScooterId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RideStartTime).NotEmpty().LessThanOrEqualTo(DateTime.Now);
        RuleFor(x => x.RideEndTime).NotEmpty().GreaterThan(x => x.RideStartTime);
    }
}
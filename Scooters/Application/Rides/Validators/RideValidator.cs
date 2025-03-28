namespace Application.Rides.Validators;

public class RideValidator : AbstractValidator<Ride>
{
    public RideValidator()
    {
        RuleFor(x => x.ScooterId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
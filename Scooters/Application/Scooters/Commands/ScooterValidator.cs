namespace Application.Scooters.Commands;

public class ScooterValidator : AbstractValidator<Scooter>
{
    public ScooterValidator()
    {
        RuleFor(x => x.ModelDescription).NotEmpty();
        RuleFor(x => x.Coordinates).NotEmpty();
    }
}
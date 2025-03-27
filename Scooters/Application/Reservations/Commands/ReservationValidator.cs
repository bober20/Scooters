namespace Application.Reservations.Commands;

public class ReservationValidator : AbstractValidator<Reservation>
{
    public ReservationValidator()
    {
        RuleFor(x => x.ScooterId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ReservationEndTime).NotEmpty().GreaterThanOrEqualTo(DateTime.Now);
    }
}
namespace Application.Common.Interfaces;

public interface INavigationService
{
    public Task NavigateToLoginPageAsync();
    public Task NavigateToSignUpPageAsync();
    public Task NavigateToPasswordChangePageAsync();
    public Task NavigateToProfilePageAsync();
    public Task NavigateToMapPageAsync();
    public Task NavigateToReservationPageAsync(Scooter scooter);
    public Task NavigateToRidePageAsync(Ride ride);
    public Task GoBackAsync();
}
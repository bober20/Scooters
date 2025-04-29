using Application.Common.Interfaces;
using CommunityToolkit.Maui.Core;
using Domain.Entities;
using Scooters.ViewModels;
using Scooters.Views;

namespace Scooters.Common.Services;

public class NavigationService : INavigationService
{
    private IServiceProvider _serviceProvider;
    private TaskCompletionSource<bool>? _popupCompletionSource;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public Task NavigateToLoginPageAsync()
    {
        return NavigateToAsync($"//{nameof(LoginPage)}");
    }

    public Task NavigateToSignUpPageAsync()
    {
        return NavigateToAsync($"//{nameof(SignUpPage)}");
    }

    public Task NavigateToPasswordChangePageAsync()
    {
        return NavigateToAsync($"{nameof(PasswordChangePage)}");
    }

    public Task NavigateToProfilePageAsync()
    {
        return NavigateToAsync($"//{nameof(ProfilePage)}");
    }

    public Task NavigateToMapPageAsync()
    {
        return NavigateToAsync($"//{nameof(ScootersMapPage)}");
    }

    public Task NavigateToReservationPageAsync(Scooter scooter)
    {
        var viewModel = _serviceProvider.GetRequiredService<ReservationViewModel>();
        viewModel.SetScooter(scooter);
        var page = new ReservationPage(viewModel);
    
        return Shell.Current.Navigation.PushModalAsync(page, true);
    }

    public Task NavigateToRidePageAsync(Ride ride)
    {
        var viewModel = _serviceProvider.GetRequiredService<RideViewModel>();
        viewModel.SetRide(ride);
        var page = new RidePage(viewModel);
    
        return Shell.Current.Navigation.PushModalAsync(page, true);
    }

    public async Task GoBackAsync()
    {
        await Shell.Current.Navigation.PopModalAsync();
    }

    private async Task NavigateToAsync(string page, IDictionary<string, object> parameters = null)
    {
        if (parameters is null)
        {
            await MainThread.InvokeOnMainThreadAsync(async () => { await Shell.Current.GoToAsync(page); });
            return;
        }

        await MainThread.InvokeOnMainThreadAsync(async () => { await Shell.Current.GoToAsync(page, parameters); });
    }
}
using Application.Common.Interfaces;
using CommunityToolkit.Maui.Core;
using Scooters.Views;

namespace Scooters.Common.Services;

public class NavigationService : INavigationService
{
    private IPopupService _popupService;

    public NavigationService(IPopupService popupService)
    {
        _popupService = popupService;
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

    public async Task ShowPopupAsync<TViewModel>(Action<TViewModel> onPresenting)
        where TViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await _popupService.ShowPopupAsync<TViewModel>(onPresenting);
        });
    }

    public async Task ShowPopupAsync<TViewModel>()
        where TViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await _popupService.ShowPopupAsync<TViewModel>();
        });
    }

    public async Task ClosePopupAsync()
    {
        await MainThread.InvokeOnMainThreadAsync(() => _popupService.ClosePopupAsync());
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
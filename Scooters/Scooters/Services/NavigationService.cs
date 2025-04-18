using Application.Common.Interfaces.NavigationService;
using CommunityToolkit.Maui.Core;
using Scooters.Views;

namespace Scooters.Services;

public class NavigationService : INavigationService
{
    private IPopupService _popupService;

    public NavigationService(IPopupService popupService)
    {
        _popupService = popupService;
    }
    
    public Task NavigateToMainPageAsync()
    {
        return NavigateToAsync($"//{nameof(MainPage)}");
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

    public Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        return MainThread.InvokeOnMainThreadAsync(() =>
            Shell.Current.DisplayAlert(title, message, cancel));
    }

    public Task<string?> ShowOptionsAsync(string title, string cancel, params string[] options)
    {
        return MainThread.InvokeOnMainThreadAsync(() =>
            Shell.Current.DisplayActionSheet(title, cancel, null, options));
    }

    public Task<string?> PromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel",
        string placeholder = "", int maxLength = -1)
    {
        return MainThread.InvokeOnMainThreadAsync(async () =>
            await Shell.Current.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength));
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
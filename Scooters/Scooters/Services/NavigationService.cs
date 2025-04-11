using Application.Common.Interfaces.NavigationService;

namespace Scooters.Services;

public class NavigationService : INavigationService
{
    public async Task NavigateToAsync(string page, IDictionary<string, object> parameters = null)
    {
        if (parameters is null)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.GoToAsync(page);
            });
            return;
        }
        
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.GoToAsync(page, parameters);
        });
    }

    public async Task GoBackAsync()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.Navigation.PopAsync();
        });
    }
}
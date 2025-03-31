using Application.Common.Interfaces.CurrentUserProvider;

namespace Scooters.Services;

public class CurrentUserProvider : ICurrentUserProvider
{
    public string GetCurrentUserAsync()
    {
        return Preferences.Get("oauth_token", null);
    }

    public void SetCurrentUserAsync(string token)
    {
        Preferences.Set("oauth_token", token);
    }

    public void RemoveCurrentUser()
    {
        SecureStorage.Remove("oauth_token");
    }
}
using Application.Common.Interfaces.CurrentUserProvider;

namespace Scooters.Services;

public class CurrentUserProvider : ICurrentUserProvider
{
    public async Task<string?> GetCurrentUserAsync()
    {
        return await SecureStorage.GetAsync("oauth_token");
    }

    public async Task SetCurrentUserAsync(string token)
    {
        await SecureStorage.SetAsync("oauth_token", token);
    }

    public void RemoveCurrentUser()
    {
        SecureStorage.Remove("oauth_token");
    }
}
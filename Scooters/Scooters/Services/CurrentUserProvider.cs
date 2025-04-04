using Application.Common.Interfaces.CurrentUserProvider;
using Application.Common.Interfaces.JwtTokenValidator;
using Domain.Entities;

namespace Scooters.Services;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IJwtTokenValidator _jwtTokenValidator;

    public CurrentUserProvider(IJwtTokenValidator jwtTokenValidator)
    {
        _jwtTokenValidator = jwtTokenValidator;
    }

    public Guid? GetCurrentUser()
    {
        var token = Preferences.Get("oauth_token", null);
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        try
        {
            var guid = _jwtTokenValidator.ValidateToken(token);
            return guid;
        }
        catch
        {
            RemoveCurrentUser();
            return null;
        }
    }

    public void SetCurrentUser(string token)
    {
        Preferences.Set("oauth_token", token);
    }

    public void RemoveCurrentUser()
    {
        Preferences.Remove("oauth_token");
    }
}
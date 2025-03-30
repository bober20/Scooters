namespace Application.Common.Interfaces.CurrentUserProvider;

public interface ICurrentUserProvider
{
    Task<string?> GetCurrentUserAsync();
    Task SetCurrentUserAsync(string token);
    void RemoveCurrentUser();
}